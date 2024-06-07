using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Unrez
{
    [RequireComponent(typeof(Cat))]
    public class MotionController : NetworkBehaviour
    {
        private Cat _cat;

        [Header("References")]
        [SerializeField]
        private InputReader _inputReader;
        [SerializeField]
        private Transform _transform;
        [SerializeField]
        private Rigidbody2D _rb;
        [SerializeField]
        private Animator _animator;

        [Header("Settings")]
        [SerializeField]
        [Tooltip("MaxVelocity. Force will be calculated to use with AddForce (_force = _maxVelocity * _rb.drag). Linear Drag should be > 0")]
        private float _maxVelocity = 10;
        [Tooltip("Linear Drag when IsMoving (Input != 0 )")]
        [SerializeField]
        private float _acceleration = 5;
        [Tooltip("Linear Drag when !IsMoving (Input == 0)")]
        [SerializeField]
        private float _deceleration = 10;
        [SerializeField]
        private float _awayTime = 10;
        private Vector2 _movementInput;

        [Header("Debugs")]
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

        private void Awake()
        {
            _cat = GetComponent<Cat>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsOwner)
            {
                return;
            }
            _cat = GetComponent<Cat>();
            _inputReader.OnMoveEvent += OnMoveHandler;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (!IsOwner)
            {
                return;
            }
            _inputReader.OnMoveEvent -= OnMoveHandler;
        }

        private void OnMoveHandler(Vector2 movementInput)
        {
            if (!IsOwner)
            {
                return;
            }
            if (_movementInput != movementInput)
            {
                _movementInput = movementInput;
            }
        }

        private Vector2 _currentDirection;
        private void FixedUpdate()
        {
            if (!IsOwner)
            {
                return;
            }
            if (_isDashing)
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
                _spawnBarrier = _transform.position * (_currentDirection * -1.5f);
            }
            else
            {
                _currentDirection = Vector2.zero;
                _rb.drag = _deceleration;
                _force = _maxVelocity * _rb.drag;
                _rb.AddForce(Vector2.zero, ForceMode2D.Force);
            }
            Animate();
            //AnimaterServerRpc();
        }

        private void Animate()
        {
            _animator.SetBool(AnimatorParameter.IS_MOVING, _isMoving);
            _animator.SetFloat(AnimatorParameter.CAT_SIDE, (int)_lastCatSide);
        }

        [ServerRpc]
        private void AnimaterServerRpc()
        {
            AnimaterClientRpc();
        }

        [ClientRpc]
        private void AnimaterClientRpc()
        {
            if (IsOwner)
            {
                return;
            }
            Animate();
        }

        public void ApplyDash()
        {
            if (!_canDash || !_isMoving)
            {
                return;
            }
            StartCoroutine(Dashing());

        }

        private bool _canDash = true;
        [SerializeField]
        private bool _isDashing = false;
        private float _dashCooldown = 3;
        private float _dashDuration = 0.3f;
        [SerializeField]
        private float _dashCharge = 1;
        [SerializeField]
        private float _dashForce;
        private IEnumerator Dashing()
        {
            _canDash = false;
            _isDashing = true;
            if (!_isMoving)
            {
                switch (_lastCatSide)
                {
                    case CatSide.CAT_UP:
                        _currentDirection = new Vector2(1, 0);
                        break;
                    case CatSide.CAT_DOWN:
                        _currentDirection = new Vector2(-1, 0);
                        break;
                    case CatSide.CAT_RIGHT:
                        _currentDirection = new Vector2(1, 0);
                        break;
                    case CatSide.CAT_LEFT:
                        _currentDirection = new Vector2(-1, 0);
                        break;
                }
            }
            _rb.drag = 0;
            _rb.AddForce(_currentDirection * _dashForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(_dashDuration);

            _isDashing = false;
            //cooldown
            _dashCharge = 0;
            while (_dashCharge < 1)
            {
                _dashCharge += Time.deltaTime / _dashCooldown;
                yield return null;
            }
            _dashCharge = 1;
            _canDash = true;
        }


        public void CreateBarrier()
        {
            if (!_canCreateBarrier)
            {
                return;
            }
            StartCoroutine(CreatingBarrier());

        }
        [SerializeField]
        private bool _canCreateBarrier = true;
        private bool _isCreatingBarrier = false;
        private float _barrierCooldown = 3;
        [SerializeField]
        private float _barrierCharge = 1;
        [SerializeField]
        private Barrier _prefabBarrier;
        [SerializeField]
        private Vector2 _spawnBarrier;
        private IEnumerator CreatingBarrier()
        {
            _canCreateBarrier = false;
            _isCreatingBarrier = true;
            //InstantiateBarrier(_spawnBarrier);
            InstantiateBarrierServerRpc(_spawnBarrier);
            _isCreatingBarrier = false;
            //cooldown
            _barrierCharge = 0;
            while (_barrierCharge < 1)
            {
                _barrierCharge += Time.deltaTime / _barrierCooldown;
                yield return null;
            }
            _barrierCharge = 1;
            _canCreateBarrier = true;
        }

        [ServerRpc]
        private void InstantiateBarrierServerRpc(Vector2 spawn)
        {
            Debug.Log($"CreateBarrierServerRpc IsServer:{IsServer}");
            Barrier barrier = Instantiate(_prefabBarrier, spawn, Quaternion.identity);
            barrier.GetComponent<NetworkObject>().Spawn();
            barrier.SetOwnerClientRpc(_cat.GetData().OwnerId, _cat.GetData().Color);

            //InstantiateBarrierClientRpc(spawn);
        }



        /*
        [ClientRpc]
        private void InstantiateBarrierClientRpc(Vector2 spawn)
        {
            if (IsOwner)
            {
                return;
            }
            InstantiateBarrier(spawn);
        }

        private void InstantiateBarrier(Vector2 spawn)
        {
            Barrier barrier = Instantiate(_prefabBarrier, spawn, Quaternion.identity);
            barrier.SetOwner(_cat.GetData().OwnerId, _cat.GetData().Color);
        }
        */

        public bool IsDashing()
        {
            return _isDashing;
        }

        public bool CanDash()
        {
            return _canDash;
        }
    }
}