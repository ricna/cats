using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using Unrez;

namespace Unrez
{
    [RequireComponent(typeof(Cat))]
    public class CatInputHandler : NetworkBehaviour
    {
        private Cat _cat; 
        
        [Header("References")]
        [SerializeField]
        private InputReader _inputReader;
        private Vector2 _movementInput;

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
            _inputReader.OnMoveEvent += OnMoveHandler;
            _inputReader.OnDashEvent += OnDashHandler;
            _inputReader.OnBarrierEvent += OnBarrierHandler;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (!IsOwner)
            {
                return;
            }
            _inputReader.OnMoveEvent -= OnMoveHandler;
            _inputReader.OnDashEvent -= OnDashHandler;
            _inputReader.OnBarrierEvent -= OnBarrierHandler;
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
                _cat.SetMovementInput(_movementInput);
            }
        }

        private void OnDashHandler()
        {
            if (!IsOwner)
            {
                return;
            }
            _cat.Dash();
        }

        private void OnBarrierHandler()
        {
            if (!IsOwner)
            {
                return;
            }
            _cat.CreateBarrier();
        }
    }

}