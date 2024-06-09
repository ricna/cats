using Unity.Netcode;
using UnityEngine;

namespace Unrez.Pets.Cats
{
    [RequireComponent(typeof(Cat))]
    public class PetInputHandler : NetworkBehaviour
    {
        private Pet _pet; 
        
        [Header("References")]
        [SerializeField]
        private CatInputReader _inputReader;
        private Vector2 _movementInput;

        private void Awake()
        {
            _pet = GetComponent<Cat>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsOwner)
            {
                return;
            }
            _inputReader.OnMoveEvent += OnMoveHandler;
            _inputReader.OnAbility01Event += OnAbility01Handler;
            _inputReader.OnAbility02Event += OnAbility02Handler;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (!IsOwner)
            {
                return;
            }
            _inputReader.OnMoveEvent -= OnMoveHandler;
            _inputReader.OnAbility01Event -= OnAbility01Handler;
            _inputReader.OnAbility02Event -= OnAbility02Handler;
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

        private void OnAbility01Handler()
        {
            if (!IsOwner)
            {
                return;
            }
            _pet.TryAbility01();
        }

        private void OnAbility02Handler()
        {
            if (!IsOwner)
            {
                return;
            }
            _pet.TryAbility02();
        }

        private void OnAbility03Handler()
        {
            if (!IsOwner)
            {
                return;
            }
            _pet.TryAbility03();
        }

        private void OnAbility04Handler()
        {
            if (!IsOwner)
            {
                return;
            }
            _pet.TryAbility04();
        }
    }

}