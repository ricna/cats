
using System.Collections;
using UnityEngine;

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
            _isCrouched = crouch;
        }

        public override void SetSprintInput(bool sprint)
        {
            _isSprinting = sprint;
            if (_isSprinting)
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