using System;
using Unity.Netcode;
using UnityEngine;

namespace Unrez.BackyardShowdown
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
        [SerializeField]
        protected SpriteRenderer _spriteRenderBody;

        [Header("Sprite Rotation")]
        [SerializeField]
        private float _rotationSpeed = 10f;

        [Header("Debugs")]
        protected Vector2 _movementInput;
        [SerializeField]
        protected float _force;
        [SerializeField]
        protected PetSide _petSide;
        [SerializeField]
        protected bool _isMoving;
        [SerializeField]
        protected bool _inputCrouch = false;
        [SerializeField]
        protected bool _isCrouched;
        [SerializeField]
        protected bool _inputSprint = false;
        [SerializeField]
        protected bool _isSprinting;
        protected bool _isMovingVertical = false;
        protected bool _isMovingHorizontal = false;
        [SerializeField]
        protected Vector2 _currentDirection;
        protected Vector2 _lastDirection;

        // Variáveis de Rede Sincronizadas
        private readonly NetworkVariable<PetSide> _netPetSide = new NetworkVariable<PetSide>(PetSide.None);
        private readonly NetworkVariable<Vector2> _netDirection = new NetworkVariable<Vector2>(Vector2.zero);

        // Propriedade pública para expor a variável de rede.
        public NetworkVariable<Vector2> NetDirection => _netDirection;

        // Events
        public event Action<Vector2> OnDirectionChangedEvent;
        public event Action<bool> OnSprintChangedEvent;
        public event Action<bool> OnCrouchChangedEvent;

        protected virtual void Awake()
        {
            _pet = GetComponent<Pet>();
            if (_transform == null)
            {
                _transform = transform;
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _netPetSide.OnValueChanged += OnPetSideChanged;
            _netDirection.OnValueChanged += OnDirectionChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            _netPetSide.OnValueChanged -= OnPetSideChanged;
            _netDirection.OnValueChanged -= OnDirectionChanged;
        }

        private void OnPetSideChanged(PetSide oldSide, PetSide newSide)
        {
            if (!IsOwner)
            {
                _petSide = newSide;
                Animate();
            }
        }

        private void OnDirectionChanged(Vector2 oldDirection, Vector2 newDirection)
        {
            if (!IsOwner)
            {
                _currentDirection = newDirection;
            }
        }

        protected virtual void FixedUpdate()
        {
            if (!IsOwner)
            {
                UpdateSpriteRotation();
                Animate();
                return;
            }
            if (_pet == null)
            {
                return;
            }

            ProcessLocalInputs();
            UpdateSpriteRotation();
            Animate();

            UpdateNetworkStateServerRpc(_movementInput, _petSide);
        }

        [ServerRpc]
        private void UpdateNetworkStateServerRpc(Vector2 movementInput, PetSide petSide)
        {
            _netDirection.Value = movementInput;
            _netPetSide.Value = petSide;
        }

        private void ProcessLocalInputs()
        {
            if (!_pet.CanSprint() && !_pet.CanCrouch())
            {
                _isSprinting = false;
                _isCrouched = false;
                return;
            }

            _isMovingHorizontal = _movementInput.x != 0;
            _isMovingVertical = _movementInput.y != 0;
            _isMoving = _isMovingHorizontal || _isMovingVertical;

            if (_isMoving)
            {
                _rb.linearDamping = _pet.Profile.Acceleration;
                HandleMovementState();

                _currentDirection = _movementInput;
                if (_currentDirection != Vector2.zero)
                {
                    _lastDirection = _currentDirection;
                }

                UpdatePetSide();
                _rb.AddForce(_force * _currentDirection, ForceMode2D.Force);
                OnDirectionChangedEvent?.Invoke(_currentDirection);
            }
            else
            {
                _isMoving = false;
                if (_currentDirection != Vector2.zero)
                {
                    _currentDirection = Vector2.zero;
                    _rb.linearDamping = _pet.Profile.Deceleration;
                    OnDirectionChangedEvent?.Invoke(_currentDirection);
                }
                _petSide = PetSide.None;
            }
        }

        private void HandleMovementState()
        {
            if (_inputSprint)
            {
                if (!_isSprinting)
                {
                    _isSprinting = true;
                    OnSprintChangedEvent?.Invoke(true);
                }
                if (_isCrouched)
                {
                    _isCrouched = false;
                    OnCrouchChangedEvent?.Invoke(false);
                }
                _force = _pet.Profile.SpeedSprint * _rb.linearDamping;
            }
            else if (_inputCrouch)
            {
                if (_isSprinting)
                {
                    _isSprinting = false;
                    OnSprintChangedEvent?.Invoke(false);
                }
                if (!_isCrouched)
                {
                    _isCrouched = true;
                    OnCrouchChangedEvent?.Invoke(true);
                }
                _force = _pet.Profile.SpeedCrouch * _rb.linearDamping;
            }
            else
            {
                if (_isCrouched)
                {
                    _isCrouched = false;
                    OnCrouchChangedEvent?.Invoke(false);
                }
                if (_isSprinting)
                {
                    _isSprinting = false;
                    OnSprintChangedEvent?.Invoke(false);
                }
                _force = _pet.Profile.Speed * _rb.linearDamping;
            }
        }

        private void UpdatePetSide()
        {
            if (_isMovingVertical && Mathf.Abs(_movementInput.y) >= Mathf.Abs(_movementInput.x))
            {
                _petSide = _movementInput.y > 0 ? PetSide.North : PetSide.South;
            }
            else if (_isMovingHorizontal && Mathf.Abs(_movementInput.x) > Mathf.Abs(_movementInput.y))
            {
                _petSide = _movementInput.x > 0 ? PetSide.East : PetSide.West;
            }

            if (_isMovingVertical && _isMovingHorizontal)
            {
                if (_movementInput.x > 0 && _movementInput.y > 0) _petSide = PetSide.NorthEast;
                else if (_movementInput.x < 0 && _movementInput.y > 0) _petSide = PetSide.NorthWest;
                else if (_movementInput.x > 0 && _movementInput.y < 0) _petSide = PetSide.SouthEast;
                else if (_movementInput.x < 0 && _movementInput.y < 0) _petSide = PetSide.SouthWest;
            }
        }

        private void UpdateSpriteRotation()
        {
            Vector2 directionToRotate;

            if (IsOwner)
            {
                directionToRotate = _isMoving ? _movementInput : _lastDirection;
            }
            else
            {
                directionToRotate = _netDirection.Value;
            }

            if (directionToRotate != Vector2.zero)
            {
                float angle = Mathf.Atan2(directionToRotate.y, directionToRotate.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                _spriteRenderBody.transform.rotation = Quaternion.Slerp(_spriteRenderBody.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }

        protected virtual void Animate()
        {
            _animator.SetBool(AnimatorParameter.IS_MOVING, _isMoving);
            _animator.SetBool(AnimatorParameter.IS_CROUCHED, _isCrouched);
            _animator.SetBool(AnimatorParameter.IS_SPRINTING, _isSprinting);
            _animator.SetFloat(AnimatorParameter.PET_SIDE, (int)_petSide);
        }

        public virtual void ApplyImpulse(float impulse, float newLinearDrag = -1, bool useNewDirection = false, float newDirX = 0, float newDirY = 0)
        {
            if (newLinearDrag != -1)
            {
                _rb.linearDamping = 0;
            }
            Vector2 direction;
            if (useNewDirection)
            {
                direction = new Vector2(newDirX, newDirY);
            }
            else
            {
                direction = _lastDirection;
            }
            _rb.AddForce(direction * impulse, ForceMode2D.Impulse);
        }

        public bool IsMoving()
        {
            return _isMoving;
        }

        public bool IsSprinting()
        {
            return _isSprinting;
        }

        public Vector2 GetDirection()
        {
            return _lastDirection;
        }

        public PetSide GetPetSide()
        {
            return _petSide;
        }

        public void SetMovementInput(Vector2 movementInput)
        {
            _movementInput = movementInput;
        }

        public virtual void SetSprintInput(bool sprint)
        {
            if (!_pet.CanSprint())
            {
                sprint = false;
            }
            _inputSprint = sprint;
        }

        public virtual void SetCrouchInput(bool crouch)
        {
            if (!_pet.CanCrouch())
            {
                crouch = false;
            }
            _inputCrouch = crouch;
        }
    }
}