using System;
using Unity.Netcode;
using UnityEngine;

namespace Unrez.Pets.Cats
{
    public class PetMotion : NetworkBehaviour
    {
        protected Pet _pet;

        [Header("References")]
        [SerializeField]
        protected Transform _transform;
        [SerializeField]
        protected Rigidbody2D _rb;
        [SerializeField]
        protected Animator _animator;

        [Header("Debugs")]
        protected Vector2 _movementInput;
        [SerializeField]
        protected float _force;
        [SerializeField]
        protected PetSide _petSide;
        [SerializeField]
        protected PetSide _lastPetSide;
        [SerializeField]
        protected bool _isMoving;
        protected bool _isMovingVertical = false;
        protected bool _isMovingHorizontal = false;
        [SerializeField]
        protected Vector2 _currentDirection;

        //Events
        public event Action<Vector2> OnDirectionChangedEvent;

        protected virtual void Awake()
        {
            _pet = GetComponent<Pet>();
        }

        protected virtual void FixedUpdate()
        {
            if (!IsOwner)
            {
                return;
            }
            if (_pet == null)
            {
                return;
            }
            _isMovingHorizontal = _movementInput.x != 0;
            _isMovingVertical = _movementInput.y != 0;
            _isMoving = _isMovingHorizontal || _isMovingVertical;
            if (_isMoving)
            {
                _rb.drag = _pet.Profile.Acceleration;
                _force = _pet.Profile.SpeedSprint * _rb.drag;
                if (_isMovingVertical)
                {
                    _currentDirection = Vector2.up * _movementInput.y;
                    _petSide = _movementInput.y > 0 ? PetSide.CAT_UP : PetSide.CAT_DOWN;
                }
                if (_isMovingHorizontal)
                {
                    if (_isMovingVertical)
                    {
                        _currentDirection = Vector2.up * _movementInput.y + Vector2.right * _movementInput.x;

                    }
                    else
                    {
                        _currentDirection = Vector2.right * _movementInput.x;
                    }
                    _petSide = _movementInput.x > 0 ? PetSide.CAT_RIGHT : PetSide.CAT_LEFT;
                }
                if (_lastPetSide != _petSide)
                {
                    _lastPetSide = _petSide;
                }
                _rb.AddForce(_force * _currentDirection, ForceMode2D.Force);
                OnDirectionChangedEvent?.Invoke(_currentDirection);
            }
            else
            {
                if (_currentDirection != Vector2.zero)
                {
                    _currentDirection = Vector2.zero;
                    _rb.drag = _pet.Profile.Deceleration;
                    _force = _pet.Profile.SpeedSprint * _rb.drag;
                    _rb.AddForce(Vector2.zero, ForceMode2D.Force);
                    OnDirectionChangedEvent?.Invoke(_currentDirection);
                }
            }
            Animate();
        }

        protected virtual void Animate()
        {
            _animator.SetBool(AnimatorParameter.IS_MOVING, _isMoving);
            _animator.SetFloat(AnimatorParameter.CAT_SIDE, (int)_lastPetSide);
        }

        public virtual void ApplyImpulse(float impulse, float newLinearDrag = -1, bool useNewDirection = false, float newDirX = 0, float newDirY = 0)
        {
            if (newLinearDrag != -1)
            {
                _rb.drag = 0;
            }
            Vector2 direction;
            if (useNewDirection)
            {
                direction = new Vector2(newDirX, newDirY);
            }
            else
            {
                if (_pet.IsMoving())
                {
                    direction = _currentDirection;
                }
                else
                {
                    switch (_lastPetSide)
                    {
                        case PetSide.CAT_UP:
                            direction = new Vector2(1, 0);
                            break;
                        case PetSide.CAT_DOWN:
                            direction = new Vector2(-1, 0);
                            break;
                        case PetSide.CAT_RIGHT:
                            direction = new Vector2(1, 0);
                            break;
                        case PetSide.CAT_LEFT:
                            direction = new Vector2(-1, 0);
                            break;
                        default:
                            direction = _currentDirection;
                            break;
                    }
                }
            }
            _rb.AddForce(direction * impulse, ForceMode2D.Impulse);
        }

        public bool IsMoving()
        {
            return _isMoving;
        }
        public Vector2 GetCurrentDirection()
        {
            return _currentDirection;
        }
        public void SetMovementInput(Vector2 movementInput)
        {
            _movementInput = movementInput;
        }
    }
}
