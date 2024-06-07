using Unity.Netcode.Components;

namespace Unrez
{
    public class ClientNetworkTransform : NetworkTransform
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

                /*   if (!IsHost && IsOwner && NetworkManager.IsConnectedClient)
               {
                   TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
               }
                */
            }
        }

        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}