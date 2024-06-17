using System;
using Unity.Netcode;
using UnityEngine;
using Unrez.Pets.Cats;

namespace Unrez.Pets
{
    [RequireComponent(typeof(Pet))]
    public class PetInputHandler : NetworkBehaviour
    {
        private Pet _pet;

        [Header("References")]
        [SerializeField]
        private InputReader _inputReader;
        private Vector2 _movementInput;

        private void Awake()
        {
            _pet = GetComponent<Pet>();
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
                _pet.SetMovementInput(_movementInput);
            }
        }

        private void OnSprintHandler(bool pressing)
        {
            if (!IsOwner)
            {
                return;
            }

            _pet.SetSprintInput(pressing);
        }

        private void OnCrouchHandler(bool pressing)
        {
            if (!IsOwner)
            {
                return;
            }
            _pet.SetCrouchInput(pressing);
        }

        private void OnAbilityHandler(int abilityID)
        {
            if (!IsOwner)
            {
                return;
            }
            _pet.TryAbility(abilityID);
        }

        private void OnInteractHandler(bool pressing)
        {
            if (!IsOwner)
            {
                return;
            }
            _pet.ProcessInteractInput(pressing);
        }
    }
}