using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;

namespace Unrez
{

    public static class AnimatorParameter
    {
        public static readonly string CAT_SIDE = "CatSide";
        public static readonly string IS_MOVING = "IsMoving";
        public static readonly string AWAY = "Away";
        public static readonly string SLEEPING = "Sleeping";
    }

    public enum CatSide
    {
        NO_SIDE = -1,
        CAT_UP = 0,
        CAT_DOWN = 1,
        CAT_RIGHT = 2,
        CAT_LEFT = 3,
    }

    public class MotionController : NetworkBehaviour
    {
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

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                return;
            }
            _inputReader.OnMoveEvent += OnMove;
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner)
            {
                return;
            }
            _inputReader.OnMoveEvent -= OnMove;

        }

        private void OnMove(Vector2 movementInput)
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

        private void FixedUpdate()
        {
            if (!IsOwner)
            {
                return;
            }
            Vector2 dir = Vector2.zero;
            _isMovingHorizontal = _movementInput.x != 0;
            _isMovingVertical = _movementInput.y != 0;
            _isMoving = _isMovingHorizontal || _isMovingVertical;
            if (_isMoving)
            {
                _rb.drag = _acceleration;
                _force = _maxVelocity * _rb.drag;
                if (_isMovingVertical)
                {
                    dir = Vector2.up * _movementInput.y;
                    _catSide = _movementInput.y > 0 ? CatSide.CAT_UP : CatSide.CAT_DOWN;
                }
                if (_isMovingHorizontal)
                {
                    if (_isMovingVertical)
                    {
                        dir = Vector2.up * _movementInput.y + Vector2.right * _movementInput.x;

                    }
                    else
                    {
                        dir = Vector2.right * _movementInput.x;
                    }
                    _catSide = _movementInput.x > 0 ? CatSide.CAT_RIGHT : CatSide.CAT_LEFT;
                }
                if (_lastCatSide != _catSide && _catSide != CatSide.NO_SIDE)
                {
                    _lastCatSide = _catSide;
                }
                _rb.AddForce(_force * dir, ForceMode2D.Force);
            }
            else
            {
                _rb.drag = _deceleration;
                _force = _maxVelocity * _rb.drag;
                _rb.AddForce(Vector2.zero, ForceMode2D.Force);
                _catSide = CatSide.NO_SIDE;
            }
            Animate();
            AnimaterServerRpc();
        }

        private void Animate()
        {
            _animator.SetBool(AnimatorParameter.IS_MOVING, _isMoving);
            _animator.SetFloat(AnimatorParameter.CAT_SIDE, (int)_lastCatSide);
        }

        [ServerRpc]
        private void AnimaterServerRpc()
        {
            _animator.SetBool(AnimatorParameter.IS_MOVING, _isMoving);
            _animator.SetFloat(AnimatorParameter.CAT_SIDE, (int)_lastCatSide);
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
    }
}