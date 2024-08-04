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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out DigSpot digSpot))
            {
                //Debug.Log($"TryGetComponent(out DigSpot digSpot) {digSpot.IsAvailable()}");
                if (digSpot.BoneSpot.GetProgress() < 100 && digSpot.IsAvailable())
                {
                    _spotDetectedAvailable = true;
                    _digSpot = digSpot;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _spotDetectedAvailable = false;
            if (_isDigging)
            {
                _digSpot.Release();
            }
        }

        protected override void InitializePet()
        {
            base.InitializePet();
            //_light.pointLightOuterRadius = Profile.PetView.FOV;
            //_cameraController.SetOrthoSize(Profile.PetView.FOV, 0.1f);
        }


        private void LateUpdate()
        {
            
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
                    _isDigging = true;
                    _digSpot.Interact(this);
                }
            }
            else
            {
                if (_isDigging)
                {
                    _isDigging = false;
                    _digSpot.Release();
                }
            }
        }

        public override void SetSprintInput(bool pressing)
        {
            _motionController.SetSprintInput(pressing);
        }
        
        public override void SetCrouchInput(bool crouch)
        {
            _motionController.SetCrouchInput(crouch);
        }

        public bool IsDigging()
        {
            return _isDigging;
        }
    }
}