using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBallController : MonoBehaviour
{
    public event Action<bool> EventHoleCollision = null;

    private Rigidbody2D rb = null;
    private int golfHoleLayer = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        golfHoleLayer = LayerMask.NameToLayer("GolfHole");
    }

    public void LaunchBall(Vector2 velocity)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(velocity);
    }

    public void CalculateTrajectory(Vector2 velocity)
    {
        
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
    }
}
