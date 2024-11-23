using UnityEditor;
using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public class Dog : Pet
    {
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
            _petMotion.SetSprintInput(true);
            _petMotion.SetCrouchInput(false);
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

        public override void SetCrouchInput(bool pressing)
        {
            _petMotion.SetCrouchInput(false);
        }

        public override void SetSprintInput(bool pressing)
        {
            _petMotion.SetSprintInput(true);
        }


        [Header("Hit Detection")]
        [SerializeField]
        private Cat _catDetected;
        [SerializeField]
        private float _hitDetectionDistance;
        [SerializeField]
        private LayerMask _layerHitDetection;
        private bool CanHitCat()
        {
            return _catDetected != null;
        }

        public override void TryAbility(int abilityId)
        {
            if (abilityId == 0)
            {
                Debug.Log("DOG -> Try HIT");
                Vector2 direction = GetCurrentDirection();
                RaycastHit2D hit2D = Physics2D.Raycast(GetCenter(), GetPointForward(1) * _hitDetectionDistance, _layerHitDetection);
                if (hit2D)
                {
                    Cat cat = hit2D.collider.GetComponent<Cat>();
                    if (cat != null)
                    {
                        Debug.Log("DOG HIT");
                        cat.TakeHit();
                    }
                }
                //TODO - First (prepare, animation, dash), then Check the HitCollider detecting a Cat, THEN hit
                if (CanHitCat())
                {
                    _catDetected.TakeHit(1);
                }
            }
            if (abilityId == 3)
            {

            }
            /*if (_petAbilities.CanUseAbility(abilityId))
            {
                _petAbilities.ExecuteAbility(abilityId);
            }*/
        }

        public override void TakeHit(int damage)
        {
            throw new System.NotImplementedException();
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(GetCenter(), GetPointForward(1));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(GetCenter(), 0.1f);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, 0.1f);

        }
    }
}