using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unrez.BackyardShowdown
{
    public class PawnMotion : NetworkBehaviour
    {
        protected Pawn _pawn;

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
        private float _minRotationSpeed = 2f; // Velocidade de rotação quando quase parado
        [SerializeField]
        private float _maxRotationSpeed = 15f; // Velocidade de rotação em movimento máximo

        // NOVO: Cabeçalho e variável para o controle do mouse
        [Header("Mouse Movement")]
        [Tooltip("If true, the pawn will move towards the mouse cursor on click.")]
        [SerializeField]
        private bool _followMouse = false;

        [Header("Debugs")]
        protected Vector2 _movementInput;
        [SerializeField]
        protected float _force;
        [SerializeField]
        protected PawnSide _petSide;
        [SerializeField]
        protected bool _isMoving;
        [SerializeField]
        protected bool _inputCrouch = false;
        [SerializeField]
        protected bool _isCrouched;
        [SerializeField]
        protected bool _inputFollow = false;
        [SerializeField]
        protected bool _isFollowingMouse;
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
        private readonly NetworkVariable<PawnSide> _netPetSide = new NetworkVariable<PawnSide>(PawnSide.None);
        private readonly NetworkVariable<Vector2> _netDirection = new NetworkVariable<Vector2>(Vector2.zero);
        private readonly NetworkVariable<float> _netRotation = new NetworkVariable<float>(0);

        // Propriedade pública para expor a variável de rede.
        public NetworkVariable<Vector2> NetDirection => _netDirection;

        // Events
        public event Action<Vector2> OnDirectionChangedEvent;
        public event Action<bool> OnSprintChangedEvent;
        public event Action<bool> OnCrouchChangedEvent;

        protected virtual void Awake()
        {
            _pawn = GetComponent<Pawn>();
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
            _netRotation.OnValueChanged += OnRotationChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            _netPetSide.OnValueChanged -= OnPetSideChanged;
            _netDirection.OnValueChanged -= OnDirectionChanged;
            _netRotation.OnValueChanged -= OnRotationChanged;
        }

        private void OnPetSideChanged(PawnSide oldSide, PawnSide newSide)
        {
            if (!IsOwner)
            {
                _petSide = newSide;
                Animate();
            }
        }


        private void OnRotationChanged(float oldRotation, float newRotation)
        {
            if (!IsOwner)
            {
                _lastRotation = newRotation;
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
            if (_pawn == null)
            {
                return;
            }

            // NOVO: Adiciona a chamada para o método que processa o input do mouse
            if (_followMouse)
            {
                HandleMouseInput();
            }

            ProcessLocalInputs();
            UpdateSpriteRotation();
            Animate();

            UpdateNetworkStateServerRpc(_movementInput, _petSide, _lastRotation);
        }

        private Vector2 _currentMousePosition;
        private void HandleMouseInput()
        {
            if (_inputFollow)
            {
                Vector2 mouseScreenPos = _currentMousePosition;
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPos);
                Vector2 direction = (Vector2)mouseWorldPosition - (Vector2)_transform.position;
                _movementInput = direction.normalized;
            }
            else
            {
                _movementInput = Vector2.zero;
            }
        }

        [ServerRpc]
        private void UpdateNetworkStateServerRpc(Vector2 movementInput, PawnSide petSide, float rotation)
        {
            _netDirection.Value = movementInput;
            _netPetSide.Value = petSide;
            _netRotation.Value = rotation;
        }

        private void ProcessLocalInputs()
        {
            if (!_pawn.CanSprint() && !_pawn.CanCrouch())
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
                _rb.linearDamping = _pawn.Profile.Acceleration;
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
                    _rb.linearDamping = _pawn.Profile.Deceleration;
                    OnDirectionChangedEvent?.Invoke(_currentDirection);
                }
                _petSide = PawnSide.None;
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
                _force = _pawn.Profile.SpeedSprint * _rb.linearDamping;
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
                _force = _pawn.Profile.SpeedCrouch * _rb.linearDamping;
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
                _force = _pawn.Profile.Speed * _rb.linearDamping;
            }
        }

        private void UpdatePetSide()
        {
            if (_isMovingVertical && Mathf.Abs(_movementInput.y) >= Mathf.Abs(_movementInput.x))
            {
                _petSide = _movementInput.y > 0 ? PawnSide.North : PawnSide.South;
            }
            else if (_isMovingHorizontal && Mathf.Abs(_movementInput.x) > Mathf.Abs(_movementInput.y))
            {
                _petSide = _movementInput.x > 0 ? PawnSide.East : PawnSide.West;
            }

            if (_isMovingVertical && _isMovingHorizontal)
            {
                if (_movementInput.x > 0 && _movementInput.y > 0) _petSide = PawnSide.NorthEast;
                else if (_movementInput.x < 0 && _movementInput.y > 0) _petSide = PawnSide.NorthWest;
                else if (_movementInput.x > 0 && _movementInput.y < 0) _petSide = PawnSide.SouthEast;
                else if (_movementInput.x < 0 && _movementInput.y < 0) _petSide = PawnSide.SouthWest;
            }
        }

        private float _lastRotation;
        private void UpdateSpriteRotation()
        {
            if (!IsOwner)
            {
                float currentRotationSpeed = _pawn.Profile.MaxRotationSpeed;
                float angle = _lastRotation;

                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, _lastRotation));
                _spriteRenderBody.transform.rotation = Quaternion.Slerp(_spriteRenderBody.transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);
                return;
            }

            Vector2 directionToRotate;


            directionToRotate = _isMoving ? _movementInput : _lastDirection;


            if (directionToRotate != Vector2.zero)
            {
                // 1. Pega a velocidade atual do Rigidbody.
                float currentSpeed = _rb.linearVelocity.magnitude;
                // 2. Define uma velocidade máxima de referência (usar a de sprint é uma boa ideia).
                float maxSpeedReference = _pawn.Profile.SpeedSprint;
                // 3. Calcula um fator de velocidade (um valor entre 0 e 1).
                float speedFactor = Mathf.Clamp01(currentSpeed / maxSpeedReference);
                // 4. Interpola a velocidade de rotação usando o fator de velocidade.
                float currentRotationSpeed = Mathf.Lerp(_pawn.Profile.MinRotationSpeed, _pawn.Profile.MaxRotationSpeed, speedFactor);

                //currentRotationSpeed = _pawn.Profile.MaxRotationSpeed;

                // 5. Calcula o ângulo e a rotação alvo, como antes.
                float angle = _lastRotation = Mathf.Atan2(directionToRotate.y, directionToRotate.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                _spriteRenderBody.transform.rotation = Quaternion.Slerp(_spriteRenderBody.transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);
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

        public PawnSide GetPetSide()
        {
            return _petSide;
        }

        public void SetMovementInput(Vector2 movementInput)
        {
            _movementInput = movementInput;
        }

        public virtual void SetSprintInput(bool sprint)
        {
            if (!_pawn.CanSprint())
            {
                sprint = false;
            }
            _inputSprint = sprint;
        }

        public virtual void SetCrouchInput(bool crouch)
        {
            if (!_pawn.CanCrouch())
            {
                crouch = false;
            }
            _inputCrouch = crouch;
        }

        public virtual void SetFollowMouseInput(bool follow)
        {
            if (!_pawn.CanFollow())
            {
                follow = false;
            }
            _inputFollow = follow;
        }

        public virtual void SetMousePosition(Vector2 pos)
        {
            _currentMousePosition = pos;
        }
    }
}