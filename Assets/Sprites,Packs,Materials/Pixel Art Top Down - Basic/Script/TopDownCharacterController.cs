using UnityEngine;
using Unity.Netcode;
using Cainos.PixelArtTopDown_Basic;

public class playermovement : NetworkBehaviour
{
    public float moveSpeed = 3f;
    public float sprintSpeed = 6f;

    public Rigidbody2D rb;
    private Camera _mainCamera;

    private networkHealthState healthState;

    private void Initialize()
    {
        _mainCamera = Camera.main;
        healthState = GetComponent<networkHealthState>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer && IsOwner)
        {
            // Host spawns at (6, -9)
            SetPosition(new Vector2(6f, -9f));
        }
        else if (!IsServer && IsOwner)
        {
            // Client spawns at (2, 12.53)
            SetPosition(new Vector2(2f, 12.53f));
        }

        Initialize();
    }

    private void SetPosition(Vector2 position)
    {
        transform.position = position;

        if (IsServer)
        {
            // Sync the position with all clients
            SetPositionClientRpc(position);
        }
    }

    [ClientRpc]
    private void SetPositionClientRpc(Vector2 position)
    {
        if (!IsOwner) // Ensure that the client only updates positions for other players, not itself.
        {
            transform.position = position;
        }
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Collision detected with: " + collider.name);

        if (!IsServer) return;

        var bulletScript = collider.GetComponent<BulletScript>();
        if (bulletScript)
        {
            Debug.Log("Player hit by bullet");
            // Assuming the health state is on the same object as the script
            healthState.HealthPoint.Value -= 10;
            Debug.Log("New health: " + healthState.HealthPoint.Value);
        }
    }
}