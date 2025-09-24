using System;
using Unity.Netcode;
using UnityEngine;
using Unrez.BackyardShowdown;

namespace Unrez.BackyardShowdown
{
    [RequireComponent(typeof(Pawn))]
    public class PawnInputHandler : NetworkBehaviour
    {
        private Pawn _pawn;

        [Header("References")]
        [SerializeField]
        private InputReader _inputReader;
        private Vector2 _movementInput;

        private void Awake()
        {
            _pawn = GetComponent<Pawn>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsOwner)
            {
                return;
            }
            _inputReader.OnMoveEvent += OnMoveHandler;
            _inputReader.OnSprintEvent += OnSprintHandler;
            _inputReader.OnCrouchEvent += OnCrouchHandler;
            _inputReader.OnAbilityEvent += OnAbilityHandler;
            _inputReader.OnInteractEvent += OnInteractHandler;
            _inputReader.OnMinimapEvent += OnMinimapHandler;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (!IsOwner)
            {
                return;
            }
            _inputReader.OnMoveEvent -= OnMoveHandler;
            _inputReader.OnSprintEvent -= OnSprintHandler;
            _inputReader.OnCrouchEvent -= OnCrouchHandler;
            _inputReader.OnAbilityEvent -= OnAbilityHandler;
            _inputReader.OnInteractEvent -= OnInteractHandler;
            _inputReader.OnMinimapEvent += OnMinimapHandler;

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
                _pawn.SetMovementInput(_movementInput);
            }
        }

        private void OnSprintHandler(bool pressing)
        {
            if (!IsOwner)
            {
                return;
            }

            _pawn.SetSprintInput(pressing);
        }

        private void OnCrouchHandler(bool pressing)
        {
            if (!IsOwner)
            {
                return;
            }
            _pawn.SetCrouchInput(pressing);
        }

        private void OnAbilityHandler(int abilityID)
        {
            if (!IsOwner)
            {
                return;
            }
            _pawn.TryAbility(abilityID);
        }

        private void OnInteractHandler(bool pressing)
        {
            if (!IsOwner)
            {
                return;
            }
            _pawn.ProcessInteractInput(pressing);
        }

        private void OnMinimapHandler()
        {
            if (!IsOwner)
            {
                return;
            }
            _pawn.ToggleMinimap();
        }
    }
}