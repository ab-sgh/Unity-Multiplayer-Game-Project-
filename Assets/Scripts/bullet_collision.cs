using UnityEngine;
using Unity.Netcode;

public class BulletCollision : NetworkBehaviour
{
    public GameObject hitPrefab;

    private void Start()
    {
        if (IsServer)
        {
            GetComponent<Collider2D>().isTrigger = false; // For environment collisions
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        HandleCollision(collider);
    }

    private void HandleCollision(Collider2D collider)
{
    if (!IsServer) return;

    Debug.Log("Bullet collided with: " + collider.name);

    if (collider.CompareTag("Player"))
    {
        var player = collider.GetComponent<playermovement>();
        if (player != null)
        {
            Debug.Log("Player hit by bullet");
            // Call the server RPC to update health
            var networkObjectId = player.GetComponent<NetworkObject>().NetworkObjectId;
            UpdatePlayerHealthServerRpc(networkObjectId);

            // Check if the player health reaches zero
            var playerHealthState = player.GetComponent<networkHealthState>();
            if (playerHealthState != null && playerHealthState.HealthPoint.Value <= 0)
            {
                var gameManager = FindObjectOfType<GameManager>();
                if (gameManager != null)
                {
                    gameManager.NotifyGameOverServerRpc(player.GetComponent<NetworkObject>().OwnerClientId);
                }
            }

            // Spawn hit effect on all clients
            SpawnHitEffectServerRpc(transform.position);
        }
    }
    else
    {
        Debug.Log("Bullet hit a non-player object");
        // Spawn hit effect on all clients
        SpawnHitEffectServerRpc(transform.position);
    }

    // Destroy the bullet
    NetworkObject.Despawn();
}


    [ServerRpc(RequireOwnership = false)]
    private void UpdatePlayerHealthServerRpc(ulong playerNetworkObjectId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(playerNetworkObjectId, out var networkObject))
        {
            var playerHealthState = networkObject.GetComponent<networkHealthState>();
            if (playerHealthState != null)
            {
                playerHealthState.HealthPoint.Value -= 10;
                Debug.Log("New health: " + playerHealthState.HealthPoint.Value);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnHitEffectServerRpc(Vector2 position)
    {
        SpawnHitEffectClientRpc(position);
    }

    [ClientRpc]
    private void SpawnHitEffectClientRpc(Vector2 position)
    {
        Instantiate(hitPrefab, position, Quaternion.identity);
    }
}
