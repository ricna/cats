using Unity.Netcode;
using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public class PreyEffects : NetworkBehaviour
    {
        [SerializeField]
        private PreyMotion _motion;
        [SerializeField]
        private ParticleSystem _psFootsteps;

        private void Awake()
        {
            _motion = GetComponent<PreyMotion>();
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
                _psFootsteps.Play();
            }
            else
            {
                if (_psFootsteps.isPlaying)
                {
                    _psFootsteps.Stop();
                }
            }
        }
    }
}