using System;
using UnityEngine;

namespace Unrez.Pets.Cats
{
    public class Cat : Pet
    {
        [Header("Debugs - Cat")]
        [SerializeField]
        private bool _spotDetected = false;
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
                    _spotDetected = true;
                    _digSpot = digSpot;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _spotDetected = false;
            //_digSpot = null;
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

        public override void ProcessInteractInput(bool pressing)
        {
            if (pressing)
            {
                if (_spotDetected)
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
    }
}