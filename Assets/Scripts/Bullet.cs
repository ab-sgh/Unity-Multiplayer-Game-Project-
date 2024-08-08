using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletScript : NetworkBehaviour
{
    [SerializeField]
    private float speed = 20f;

    private Rigidbody2D rb;

    private Vector2 lastKnownServerPosition;
    private Vector2 predictedPosition;
    private float interpolationFactor = 0.05f;
    private float lastUpdateTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (rb == null)
        {
            // Try to get the Rigidbody2D component again in Start if it's not assigned
            rb = GetComponent<Rigidbody2D>();
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is still missing from bullet after Start. Please ensure it is attached to the bullet prefab.");
            return;
        }

        if (IsServer)
        {
            rb.velocity = transform.up * speed;
            lastKnownServerPosition = rb.position;
            lastUpdateTime = Time.time;
            Debug.Log("Bullet spawned with velocity: " + rb.velocity);
        }
    }

    // ... (rest of the code remains the same)
}


