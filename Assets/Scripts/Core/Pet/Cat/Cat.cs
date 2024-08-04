using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public class Cat : Pet
    {
        [Header("Debugs - Cat")]
        [SerializeField]
        private bool _spotDetectedAvailable = false;
        [SerializeField]
        private bool _isDigging = false;
        [SerializeField]
        private DigSpot _digSpot;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.TryGetComponent(out DigSpot digSpot))
            {
                if (digSpot.BoneSpot.GetProgress() < 100 && digSpot.IsAvailable())
                {
                    _spotDetectedAvailable = true;
                    _digSpot = digSpot;
                }
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
            if (!_isDigging)
            {
                return;
            }
            if (collision.TryGetComponent(out DigSpot digSpot))
            {
                CancelDigging();
            }
        }

        public override void TakeHit(int damage)
        {
            _healthController.TakeDamage(damage);
        }

        public override void TryAbility(int idxAbility)
        {
            if (_abilitiesController.CanUseAbility(idxAbility))
            {
                _abilitiesController.ExecuteAbility(idxAbility);
            }
        }

        public override void SetMovementInput(Vector2 movementInput)
        {
            if (_isDigging)
            {
                base.SetMovementInput(Vector2.zero);
                return;
            }
            base.SetMovementInput(movementInput);
        }

        public bool IsExecutingSomeAbility()
        {
            return _abilitiesController.Busy();
        }

        public override void ProcessInteractInput(bool interact)
        {
            if (interact)
            {
                if (_spotDetectedAvailable)
                {
                    StartDigging();
                }
            }
            else
            {
                if (_isDigging)
                {
                    CancelDigging();
                }
            }
        }

        private void StartDigging()
        {
            _isDigging = true;
            _digSpot.Interact(this);
            CheckDiggingAnimation();
        }

        private void CancelDigging()
        {
            _spotDetectedAvailable = false;
            _isDigging = false;
            _digSpot.Release();
            CheckDiggingAnimation();
        }

        private void CheckDiggingAnimation()
        {
            _animator.SetBool(AnimatorParameter.IS_DIGGING, _isDigging);
        }

        public bool IsDigging()
        {
            return _isDigging;
        }
    }
}