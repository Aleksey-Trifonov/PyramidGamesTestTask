using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayPanel : MonoBehaviour
{
    [SerializeField]
    private Text counterText = null;
    [SerializeField]
    private GameObject gameOverPanel = null;
    [SerializeField]
    private GameObject aimButton = null;
    [SerializeField]
    private Text currentScoreText = null;
    [SerializeField]
    private Text bestScoreText = null;

    private void Start()
    {
        GameplayManager.Instance.EventGameReset += OnGameReset;
        GameplayManager.Instance.EventRoundLose += OnRoundLose;
        GameplayManager.Instance.EventRoundWin += OnRoundWin;
        GameplayManager.Instance.EvantBallLaunch += OnBallLaunch;
    }

    private void OnDisable()
    {
        if (GameplayManager.Instance != null)
        {
            GameplayManager.Instance.EventGameReset -= OnGameReset;
            GameplayManager.Instance.EventRoundLose -= OnRoundLose;
            GameplayManager.Instance.EventRoundWin -= OnRoundWin;
            GameplayManager.Instance.EvantBallLaunch -= OnBallLaunch;
        }
    }

    private void OnGameReset()
    {
        aimButton.gameObject.SetActive(true);
    }

    private void OnBallLaunch()
    {
        aimButton.gameObject.SetActive(false);
    }

    private void OnRoundWin(int currentScore)
    {
        counterText.text = currentScore.ToString();
    }

    private void OnRoundLose(int currentScore, int bestScore)
    {
        counterText.gameObject.SetActive(false);
        currentScoreText.text = "Result: " + currentScore;
        bestScoreText.text = "Best: " + bestScore;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        GameplayManager.Instance.ResetGame();
        gameOverPanel.SetActive(false);
        counterText.text = "0";
        counterText.gameObject.SetActive(true);
    }
}
