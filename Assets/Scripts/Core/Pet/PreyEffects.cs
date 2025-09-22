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

        private void LateUpdate()
        {
            if (!_motion.IsSprinting())
            {
                return;
            }
            float angle = 0;
            switch (_motion.GetPetSide())
            {
                case PawnSide.North:
                    angle = 0;
                    break;
                case PawnSide.NorthEast:
                    angle = 180;
                    break;
                case PawnSide.East:
                    angle = 90;
                    break;
                case PawnSide.SouthEast:
                    angle = -90;
                    break;
                case PawnSide.South:
                    angle = -90;
                    break;
                case PawnSide.SouthWest:
                    angle = -90;
                    break;
                case PawnSide.West:
                    angle = -90;
                    break;
                case PawnSide.NorthWest:
                    angle = -90;
                    break;
            }
            _psFootsteps.transform.rotation = Quaternion.Euler(0, 0, angle);
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