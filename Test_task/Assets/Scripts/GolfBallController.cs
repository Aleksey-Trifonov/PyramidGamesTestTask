using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GolfBallController : MonoBehaviour
{
    public event Action<bool> EventHoleCollision = null;
    public event Action EventCollision = null;

    [SerializeField]
    private GameObject trajectoryDotPrefab = null;
    [SerializeField]
    private float trajectoryTimeStep = 0.3f;

    private Rigidbody2D rb = null;
    private int golfHoleLayer = 0;
    private List<GameObject> trajectoryDots = new List<GameObject>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        golfHoleLayer = LayerMask.NameToLayer("GolfHole");
        for (int i = 0; i < 10; i++)
        {
            var trajectoryDot = Instantiate(trajectoryDotPrefab, transform);
            trajectoryDot.SetActive(false);
            trajectoryDots.Add(trajectoryDot);
        }
    }

    public void LaunchBall(Vector2 velocity)
    {
        foreach (var trajectoryDot in trajectoryDots)
        {
            trajectoryDot.SetActive(false);
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(velocity, ForceMode2D.Impulse);
    }

    public Vector2 CalculateTrajectory(Vector2 velocity)
    {
        SetTrajectoryDots(velocity);

        var activeDots = trajectoryDots.FindAll(d => d.activeSelf);
        var lastActiveDotPosition = new Vector2(float.MinValue, float.MinValue);
        foreach (var activeDot in activeDots)
        {
            if (activeDot.transform.position.x > lastActiveDotPosition.x)
            {
                lastActiveDotPosition = activeDot.transform.position;
            }
        }
        return lastActiveDotPosition;
    }

    public void PauseBall()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == golfHoleLayer)
        {
            EventHoleCollision?.Invoke(true);
        }
        else
        {
            EventCollision?.Invoke();
        }
    }

    public void SetTrajectoryDots(Vector2 forceApplied)
    {
        var timeStep = trajectoryTimeStep;
        for (int i = 0; i < trajectoryDots.Count; i++)
        {
            trajectoryDots[i].transform.position = ((Vector2)transform.position + forceApplied * timeStep) - ((-Physics2D.gravity * timeStep * timeStep) /2f);
            trajectoryDots[i].SetActive(trajectoryDots[i].transform.position.y > (transform.position.y - 1f));

            timeStep += trajectoryTimeStep;
        }
    }
}
