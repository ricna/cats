using Unity.Netcode;
using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public class CatEffects : NetworkBehaviour
    {
        [SerializeField]
        private CatMotion _motion;
        [SerializeField]
        private ParticleSystem _psFootsteps;

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

        private void LateUpdate()
        {
            if (!_motion.IsSprinting())
            {
                return;
            }
            float angle = 0;
            switch (_motion.GetPetSide())
            {
                case PetSide.North:
                    angle = 0;
                    break;
                case PetSide.NorthEast:
                    angle = 180;
                    break;
                case PetSide.East:
                    angle = 90;
                    break;
                case PetSide.SouthEast:
                    angle = -90;
                    break;
                case PetSide.South:
                    angle = -90;
                    break;
                case PetSide.SouthWest:
                    angle = -90;
                    break;
                case PetSide.West:
                    angle = -90;
                    break;
                case PetSide.NorthWest:
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