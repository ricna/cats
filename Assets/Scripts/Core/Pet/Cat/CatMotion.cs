
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Unrez.Pets.Cats
{
    public class CatMotion : PetMotion
    {
        private Cat _cat;
        [SerializeField]
        private ParticleSystem _particleDustTrail;

        protected override void Awake()
        {
            base.Awake();
            _cat = GetComponent<Cat>();

            OnSprintChangedEvent += OnSprintChangedHandler;

        }

        private void OnDisable()
        {
            OnSprintChangedEvent -= OnSprintChangedHandler;
        }

        protected override void FixedUpdate()
        {
            if (_cat.IsExecutingSomeAbility())
            {
                return;
            }
            if (_cat.IsDigging())
            {
                return;
            }
            base.FixedUpdate();
        }

        public override void SetCrouchInput(bool crouch)
        {
            if (!CanCrouch())
            {
                crouch = false;
            }
            _inputCrouch = crouch;
        }

        public override void SetSprintInput(bool sprint)
        {
            if (!CanSprint())
            {
                sprint = false;
            }
            _inputSprint = sprint;
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
                if (!_particleDustTrail.isPlaying)
                {
                    _particleDustTrail.Play();
                }
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