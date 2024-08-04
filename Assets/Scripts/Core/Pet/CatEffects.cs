using System.Globalization;
using Unity.Netcode;
using UnityEngine;

namespace Unrez.Backyard.Cats
{
    public class CatEffects : NetworkBehaviour
    {
        [SerializeField]
        private CatMotion _motion;
        [SerializeField]
        private ParticleSystem _particleDustTrail;

        private void Awake()
        {
            _motion = GetComponent<CatMotion>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                _motion.OnSprintChangedEvent += OnSprintChangedHandler;
            }
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            _motion.OnSprintChangedEvent -= OnSprintChangedHandler;
        }

        private void OnSprintChangedHandler(bool enable)
        {
            if (IsOwner)
            {
                ToggleDustTrailServerRpc(enable);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void ToggleDustTrailServerRpc(bool enable)
        {
            ToggleDustTrailClientRpc(enable);
        }

        [ClientRpc]
        private void ToggleDustTrailClientRpc(bool enable)
        {
            if (!IsOwner)
            {
                return;
            }
            if (enable)
            {
                _particleDustTrail.Play();
            }
            else
            {
                if (_particleDustTrail.isPlaying)
                {
                    _particleDustTrail.Stop();
                }
            }
        }
    }
}