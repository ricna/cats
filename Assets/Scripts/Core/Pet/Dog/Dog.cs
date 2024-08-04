using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public class Dog : Pet
    {
        [SerializeField]
        private bool _alwaysSprinting = true;
        [Header("Debugs - Dog")]
        [SerializeField]
        private bool _spotDetectedAvailable = false;
        [SerializeField]
        private bool _isReburying = false;
        [SerializeField]
        private DigSpot _digSpot;

        protected override void InitializeLocalPet()
        {
            base.InitializeLocalPet();
            _motionController.SetSprintInput(_alwaysSprinting);
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.TryGetComponent(out DigSpot digSpot))
            {
                if (!digSpot.BoneSpot.HasCats())
                {
                    _spotDetectedAvailable = true;
                    _digSpot = digSpot;
                }
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
            if (!_isReburying)
            {
                return;
            }
            if (collision.TryGetComponent(out DigSpot digSpot))
            {
                CancelReburying();
            }
        }

        public override void ProcessInteractInput(bool interact)
        {
            if (interact)
            {
                if (_spotDetectedAvailable)
                {
                    StartReburying();
                }
            }
        }

        private void StartReburying()
        {
            _isReburying = true;
            _digSpot.Interact(this);
            CheckReburyingAnimation();
        }

        private void CancelReburying()
        {
            _spotDetectedAvailable = false;
            _isReburying = false;
            _digSpot.Release();
            CheckReburyingAnimation();
        }

        private void CheckReburyingAnimation()
        {
            _animator.SetBool(AnimatorParameter.IS_REBURYING, _isReburying);
        }

        public override void SetMovementInput(Vector2 movementInput)
        {
            if (_isReburying)
            {
                base.SetMovementInput(Vector2.zero);
                return;
            }
            base.SetMovementInput(movementInput);
        }

        public override void TryAbility(int abilityId)
        {
            if (_abilitiesController.CanUseAbility(abilityId))
            {
                _abilitiesController.ExecuteAbility(abilityId);
            }
        }

        public override void TakeHit(int damage)
        {
            throw new System.NotImplementedException();
        }
    }
}