using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform firePoint;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && IsOwner)
        {
            SpawnBulletServerRPC(firePoint.position, firePoint.rotation);
        }
    }

[ServerRpc]
private void SpawnBulletServerRPC(Vector3 position, Quaternion rotation, ServerRpcParams serverRpcParams = default)
{
    GameObject instantiatedBullet = Instantiate(bulletPrefab, position, rotation);
    NetworkObject networkObject = instantiatedBullet.GetComponent<NetworkObject>();
    networkObject.Spawn(); // No ownership
    Debug.Log("Bullet spawned at position: " + position + " with rotation: " + rotation);
}



}
