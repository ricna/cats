using System.Collections;
using System.Xml;
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
        private float _speed = 5;
        [SerializeField]
        private float _awayTime = 10;

        private Vector2 _movementInput;

        [Header("Debugs")]
        [SerializeField]
        private CatSide _catSide;
        [SerializeField]
        private CatSide _lastCatSide;
        [SerializeField]
        private bool _isMoving;
        private bool _isMovingVertical = false;
        private bool _isMovingHorizontal = false;
        [SerializeField]
        private float _velocity;
        private Vector2 _posInit;
        private Vector2 _posFinal;
        private bool _checkVelocity = true;

        private void FixedUpdate()
        {
            if (!IsOwner)
            {
                return;
            }
            Vector2 newPosition = Vector2.zero;
            _isMovingHorizontal = _movementInput.x != 0;
            _isMovingVertical = _movementInput.y != 0;
            _isMoving = _isMovingHorizontal || _isMovingVertical;
            if (_isMoving)
            {
                if (_isMovingVertical)
                {
                    newPosition = (Vector2)_transform.position + (Vector2.up * _movementInput.y);
                    _catSide = _movementInput.y > 0 ? CatSide.CAT_UP : CatSide.CAT_DOWN;
                }
                if (_isMovingHorizontal)
                {
                    if (_isMovingVertical)
                    {
                        newPosition = (Vector2)_transform.position + (Vector2.up * _movementInput.y + Vector2.right * _movementInput.x);
                    }
                    else
                    {
                        newPosition = (Vector2)_transform.position + (Vector2.right * _movementInput.x);
                    }
                    _catSide = _movementInput.x > 0 ? CatSide.CAT_RIGHT : CatSide.CAT_LEFT;
                }
                if (_lastCatSide != _catSide && _catSide != CatSide.NO_SIDE)
                {
                    _lastCatSide = _catSide;
                }
                _rb.MovePosition(Vector2.Lerp(_transform.position, newPosition, _speed * Time.fixedDeltaTime));
            }
            else
            {
                _catSide = CatSide.NO_SIDE;
            }
            _animator.SetBool(AnimatorParameter.IS_MOVING, _isMoving);
            _animator.SetFloat(AnimatorParameter.CAT_SIDE, (int)_lastCatSide);
            if (_checkVelocity)
            {
                _checkVelocity = false;
                if (_coroutineCheckVelocity != null)
                {
                    StopCoroutine(_coroutineCheckVelocity);
                }
                _coroutineCheckVelocity = StartCoroutine(CheckVelocity());
            }
        }
        private Coroutine _coroutineCheckVelocity;
        [SerializeField]
        private float _time;
        [SerializeField]
        private float _distance;
        private IEnumerator CheckVelocity()
        {
            _posInit = _transform.position;
            _time = 1 * Time.deltaTime;
            yield return new WaitForSeconds(_time);
            _posFinal = _transform.position;
            _distance = (_posInit - _posFinal).magnitude;
            _velocity = _distance / _time;
            //v = Δs / Δt, onde Δs é o deslocamento e Δt é o intervalo de tempo.
            _checkVelocity = true;
        }
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
            if (IsOwner)
            {
                if (_movementInput != movementInput)
                {
                    _movementInput = movementInput;
                }
            }
        }
    }
}