using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Unrez.Netcode
{
    [RequireComponent(typeof(NetworkObject))]
    public class OwnerNetworkTransform : NetworkTransform
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            CanCommitToTransform = IsOwner;
        }

        protected override void Update()
        {
            CanCommitToTransform = IsOwner;
            base.Update();
            if (IsHost)
            {
                return;
            }
            if (NetworkManager != null)
            {
                if (NetworkManager.IsConnectedClient || NetworkManager.IsListening)
                {
                    if (CanCommitToTransform)
                    {
                        TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
                    }
                }
            }
        }

        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}