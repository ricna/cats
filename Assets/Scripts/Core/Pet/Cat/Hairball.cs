using Unity.Netcode;
using UnityEngine;
using Unrez.BackyardShowdown;

namespace Unrez.BackyardShowdown
{
    public class Hairball : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [Header("Debugs")]
        [SerializeField]
        private ulong _myOwnerId;
        [SerializeField]
        public Color _myColor;
        [SerializeField]
        public Color _hp;
        [SerializeField]
        public Color _hitsTaken;
        [SerializeField]
        private ParticleSystem _particlesDestroy;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _particlesDestroy.Play();
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        /*
         * If there is a need for a gameplay event to happen on a collision, 
         * you can listen to OnCollisionEnter function on the server and synchronize the event via Rpc(SendTo.ClientsAndHost) to all clients.
         */
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
                Ability dashAbility = cat.GetAbilityByType(typeof(AbilityDash));
                if (dashAbility != null)
                {
                    if (dashAbility.IsExecuting())
                    {
                        DestroyHairballServerRpc();
                    }
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyHairballServerRpc()
        {
            _particlesDestroy.Play();
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
}