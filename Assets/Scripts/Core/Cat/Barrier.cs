using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;
using Unrez;

public class Barrier : NetworkBehaviour
{
    [Header("References")]
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [Header("Debugs")]
    [SerializeField]
    private ulong _myOwnerId;
    [SerializeField]
    public Color _myColor;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
    }

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
                DestroyBarrierServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyBarrierServerRpc()
    {
        this.GetComponent<NetworkObject>().Despawn(true);

    }

    [ClientRpc]
    public void SetOwnerClientRpc(ulong ownerId, Color ownerColor)
    {
        name = $"Barrier of {ownerId} Color:{ownerColor}";
        Unbug.Log($"SetOwner: {ownerId}", Uncolor.Blue);
        _myOwnerId = ownerId;
        _myColor = ownerColor;
        _spriteRenderer.color = _myColor;
    }
}
