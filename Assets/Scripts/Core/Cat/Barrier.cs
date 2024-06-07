using Unity.Netcode;
using UnityEngine;
using Unrez;

public class Barrier : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        ProcessCollision(collider);
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        ProcessCollision(collider);
    }

    private void ProcessCollision(Collider2D collider)
    {
        if (collider.TryGetComponent(out Cat cat))
        {
            if (cat.IsDashing())
            {
                if (IsServer)
                {
                    this.GetComponent<NetworkObject>().Despawn();
                }
                else
                {
                    DestroyBarrierServerRpc();
                }
            }
        }
    }

    [ServerRpc]
    private void DestroyBarrierServerRpc()
    {
        DestroyBarrierClientRpc();
    }

    [ClientRpc]
    private void DestroyBarrierClientRpc()
    {
        if (!IsServer)
        {
            return;
        }
        this.GetComponent<NetworkObject>().Despawn();

    }
}
