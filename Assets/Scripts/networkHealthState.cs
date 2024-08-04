using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class networkHealthState : NetworkBehaviour
{
    [HideInInspector]
    public NetworkVariable<int> HealthPoint = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
        {
            HealthPoint.Value = 100;
        }
    }
}


