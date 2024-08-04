using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cainos.PixelArtTopDown_Basic;
using System.Data.Common;
using Unity.VisualScripting;

public class playermovement : NetworkBehaviour
{
    public float moveSpeed = 3f;
    public float sprintSpeed = 6f;

    public Rigidbody2D rb;
    private Camera _mainCamera;

    private void Initialize()
    {
        _mainCamera = Camera.main;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
    }

    Vector2 movement;
    Vector2 mousePos;

    private void Start()
    {
        if (IsLocalPlayer)
        {
            var cameraFollow = Camera.main.GetComponent<CameraFollow>();
            if (cameraFollow != null)
            {
                cameraFollow.SetTarget(transform);
            }
            else
            {
                Debug.LogError("CameraFollow script not found on the main camera.");
            }
        }
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!IsOwner || !Application.isFocused) return;

        // Movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        float currentSpeed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = sprintSpeed;
        }

        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
    void OnTriggerEnter2D(Collider2D collider )
{
    if (!IsServer) return;
    if (collider.GetComponent<BulletScript>() && GetComponent<NetworkObject>().OwnerClientId != collider.GetComponent<NetworkObject>().OwnerClientId)
    {
        GetComponent<networkHealthState>().HealthPoint.Value -= 10;
    }
}

}
