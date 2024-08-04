using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletCollision : NetworkBehaviour
{
    public GameObject hitPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsServer) // Only handle the collision on the server side
        {
            Debug.Log("Bullet collided with: " + collision.gameObject.name);

            // Call the networked method to instantiate the hit effect on all clients
            SpawnHitEffectServerRpc(transform.position);
            
            // Destroy the bullet
            NetworkObject.Despawn();
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
