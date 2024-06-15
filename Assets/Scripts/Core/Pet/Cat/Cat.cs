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

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (TryGetComponent(out DigSpot digSpot))
            {
                if (!_spotDetected)
                {
                    return;
                }
                if (digSpot.BoneSpot.GetProgress() < 100 && digSpot.IsAvailable())
                {
                    _spotDetected = true;
                    _digSpot = digSpot;
                }
                else
                {
                    _spotDetected = false;
                }
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

        public bool IsExecutingSomeAbility()
        {
            return _abilitiesController.Busy();
        }

        public override void OnDigSpotEnter()
        {
            _isDigging = true;  
        }

        public override void OnDigSpotExit()
        {
            _isDigging = false;

        }

        public override void ProcessInteractInput(bool pressing)
        {
            if (pressing)
            {
                if (_spotDetected)
                {
                    _digSpot.Interact(this);
                }
            }
            else
            {
                if (_isDigging && _digSpot != null)
                {
                    _digSpot.Release();
                }
            }
        }
    }
}