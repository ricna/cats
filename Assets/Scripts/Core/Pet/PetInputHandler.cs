using Unity.Netcode;
using UnityEngine;

namespace Unrez.Pets.Cats
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
            _inputReader.OnAbilityEvent += OnAbilityHandler;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (!IsOwner)
            {
                return;
            }
            _inputReader.OnMoveEvent -= OnMoveHandler;
            _inputReader.OnAbilityEvent -= OnAbilityHandler;
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

        private void OnAbilityHandler(int abilityID)
        {
            if (!IsOwner)
            {
                return;
            }
            _pet.TryAbility(abilityID);
        }
    }
}