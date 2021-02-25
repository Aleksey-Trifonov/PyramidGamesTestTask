using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBallController : MonoBehaviour
{
    public event Action<int> EventBallCollision = null;

    private Rigidbody2D rb = null;
    private int collisionCount = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    public void LaunchBall(Vector2 velocity)
    {
        rb.isKinematic = false;
        rb.AddForce(velocity);
        collisionCount = 0;
    }

    public void CalculateTrajectory(Vector2 velocity)
    {
        
    }

    public void PauseBall()
    {
        rb.isKinematic = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionCount++;
        EventBallCollision?.Invoke(collisionCount);
        Debug.LogWarning("Collision");
    }
}
