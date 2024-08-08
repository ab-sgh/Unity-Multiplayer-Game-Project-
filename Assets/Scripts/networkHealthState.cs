using UnityEngine;
using Unity.Netcode;

public class networkHealthState : NetworkBehaviour
{
    public NetworkVariable<int> HealthPoint = new NetworkVariable<int>(100);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
        {
            HealthPoint.Value = 100; // Initialize health on server
        }
    }

    public void TakeDamage(int damage)
    {
        if (IsServer)
        {
            int oldHealth = HealthPoint.Value;
            HealthPoint.Value = Mathf.Max(HealthPoint.Value - damage, 0);
            Debug.Log($"Health updated: Old: {oldHealth}, New: {HealthPoint.Value}");
        }
    }
}
