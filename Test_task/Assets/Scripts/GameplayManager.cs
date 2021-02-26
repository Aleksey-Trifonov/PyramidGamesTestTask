using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameplayManager>();
            }
            return instance;
        }
    }

    private static GameplayManager instance = null;

    public event Action EventGameReset = null;
    public event Action<int> EventRoundWin = null;
    public event Action<int, int> EventRoundLose = null;
    public event Action EvantBallLaunch = null;

    [SerializeField] 
    private GameSettings gameSettings = null;
    [SerializeField] 
    private GolfBallController golfBallPrefab = null;
    [SerializeField] 
    private Transform golfBallStartingPoint = null;
    [SerializeField] 
    private GameObject golfHolePrefab = null;
    [SerializeField] 
    private Transform golfHoleStartingPoint = null;

    private const string BestScoreKey = "Best_score";

    private GolfBallController golfBallInstance = null;
    private GameObject golfHoleInstance = null;
    private Coroutine golfBallLifetimeCoroutine = null;
    private Vector2 currentBallVelocity = Vector2.zero;
    private int currentWinCount = 0;
    private bool isGolfBallInMotion = false;

    private void Start()
    {
        if (golfBallInstance == null)
        {
            golfBallInstance = Instantiate(golfBallPrefab, golfBallStartingPoint);
            golfBallInstance.EventHoleCollision += FinishRound;
        }

        if (golfHoleInstance == null)
        {
            golfHoleInstance = Instantiate(golfHolePrefab, golfHoleStartingPoint);
        }
        
        ResetGame();
    }

    private void OnDisable()
    {
        if (golfBallInstance != null)
        {
            golfBallInstance.EventHoleCollision -= FinishRound;
        }
    }

    private void Update()
    {
        //check if out of bounds
    }

    public void ResetGame()
    {
        golfBallInstance.transform.position = golfBallStartingPoint.position;
        golfHoleInstance.transform.position = new Vector2(
            golfHoleStartingPoint.position.x + Random.Range(-gameSettings.GolfHoleXOffset, gameSettings.GolfHoleXOffset), 
            golfHoleStartingPoint.position.y);
        currentBallVelocity = Vector2.zero;
        EventGameReset?.Invoke();
    }

    public void ChargeBallVelocity()
    {
        var winModifier = currentWinCount * gameSettings.ConsecutiveWinVelocityModifier;
        currentBallVelocity += new Vector2(gameSettings.GolfBallXVelocityIncrementRate + winModifier, 
            gameSettings.GolfBallYVelocityIncrementRate + winModifier) * Time.deltaTime;
        golfBallInstance.CalculateTrajectory(currentBallVelocity);
    }

    public void LaunchBall()
    {
        golfBallInstance.LaunchBall(currentBallVelocity);
        golfBallLifetimeCoroutine = StartCoroutine(BallLifetime());
        isGolfBallInMotion = true;
        EvantBallLaunch?.Invoke();
    }

    public void FinishRound(bool isWin)
    {
        golfBallInstance.PauseBall();
        isGolfBallInMotion = false;

        if (golfBallLifetimeCoroutine != null)
        {
            StopCoroutine(golfBallLifetimeCoroutine);
            golfBallLifetimeCoroutine = null;
        }

        if (isWin)
        {
            currentWinCount++;
            PlayerPrefs.SetInt(BestScoreKey, currentWinCount);
            EventRoundWin?.Invoke(currentWinCount);
            ResetGame();
        }
        else
        {
            EventRoundLose?.Invoke(currentWinCount, PlayerPrefs.GetInt(BestScoreKey));
            currentWinCount = 0;
        }
    }

    private IEnumerator BallLifetime()
    {
        var timer = gameSettings.GolfBallLifetime;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        FinishRound(false);
    }
}
