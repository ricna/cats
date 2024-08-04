using Unity.Netcode;
using UnityEngine;

namespace Unrez.BackyardShowdown
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
                ToggleDustTrail(enable);//display immediately
                ToggleDustTrailServerRpc(enable);//call to display for others
                
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
            if (IsOwner)
            {
                return;
            }
            ToggleDustTrail(enable);
        }

        private void ToggleDustTrail(bool enable)
        {
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