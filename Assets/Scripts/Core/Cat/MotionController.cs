using System;
using Unity.Netcode;
using UnityEngine;

namespace Unrez.Cats
{
    [RequireComponent(typeof(Cat))]
    public class MotionController : NetworkBehaviour
    {
        private Cat _cat;

        [Header("References")]
        [SerializeField]
        private Transform _transform;
        [SerializeField]
        private Rigidbody2D _rb;
        [SerializeField]
        private Animator _animator;

        [Header("Settings (Replace by CatProfile)")]
        [SerializeField]
        [Tooltip("MaxVelocity. Force will be calculated to use with AddForce (_force = _maxVelocity * _rb.drag). Linear Drag should be > 0")]
        private float _maxVelocity = 10;
        [Tooltip("Linear Drag when IsMoving (Input != 0 )")]
        [SerializeField]
        private float _acceleration = 5;
        [Tooltip("Linear Drag when !IsMoving (Input == 0)")]
        [SerializeField]
        private float _deceleration = 10;

        [Header("Debugs")]
        private Vector2 _movementInput;
        [SerializeField]
        private float _force;
        [SerializeField]
        private CatSide _catSide;
        [SerializeField]
        private CatSide _lastCatSide;
        [SerializeField]
        private bool _isMoving;
        private bool _isMovingVertical = false;
        private bool _isMovingHorizontal = false;
        [SerializeField]
        private Vector2 _currentDirection;

        //Events
        public event Action<Vector2> OnDirectionChangedEvent;

        private void Awake()
        {
            _cat = GetComponent<Cat>();
            CatProfile profile = _cat.GetProfile();
            _maxVelocity = profile.Speed;
            _acceleration = profile.Acceleration;
            _deceleration = profile.Deceleration;
        }

        private void FixedUpdate()
        {
            if (!IsOwner)
            {
                return;
            }
            if (_cat.IsDashing())
            {
                return;
            }
            _isMovingHorizontal = _movementInput.x != 0;
            _isMovingVertical = _movementInput.y != 0;
            _isMoving = _isMovingHorizontal || _isMovingVertical;
            if (_isMoving)
            {
                _rb.drag = _acceleration;
                _force = _maxVelocity * _rb.drag;
                if (_isMovingVertical)
                {
                    _currentDirection = Vector2.up * _movementInput.y;
                    _catSide = _movementInput.y > 0 ? CatSide.CAT_UP : CatSide.CAT_DOWN;
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
                    _catSide = _movementInput.x > 0 ? CatSide.CAT_RIGHT : CatSide.CAT_LEFT;
                }
                if (_lastCatSide != _catSide)
                {
                    _lastCatSide = _catSide;
                }
                _rb.AddForce(_force * _currentDirection, ForceMode2D.Force);
                OnDirectionChangedEvent?.Invoke(_currentDirection);
            }
            else
            {
                if (_currentDirection != Vector2.zero)
                {
                    _currentDirection = Vector2.zero;
                    _rb.drag = _deceleration;
                    _force = _maxVelocity * _rb.drag;
                    _rb.AddForce(Vector2.zero, ForceMode2D.Force);
                    OnDirectionChangedEvent?.Invoke(_currentDirection);
                }
            }
            Animate();
        }

        private void Animate()
        {
            _animator.SetBool(AnimatorParameter.IS_MOVING, _isMoving);
            _animator.SetFloat(AnimatorParameter.CAT_SIDE, (int)_lastCatSide);
        }

        public void ApplyImpulse(float impulse, float newLinearDrag = -1, bool useNewDirection = false, float newDirX = 0, float newDirY = 0)
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
                if (_cat.IsMoving())
                {
                    direction = _currentDirection;
                }
                else
                {
                    switch (_lastCatSide)
                    {
                        case CatSide.CAT_UP:
                            direction = new Vector2(1, 0);
                            break;
                        case CatSide.CAT_DOWN:
                            direction = new Vector2(-1, 0);
                            break;
                        case CatSide.CAT_RIGHT:
                            direction = new Vector2(1, 0);
                            break;
                        case CatSide.CAT_LEFT:
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
