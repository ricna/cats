using Unity.Netcode;
using UnityEngine;

namespace Unrez
{
    public class PlayerAiming : NetworkBehaviour
    {
        private Cat _player;

        [Header("References")]
        [SerializeField]
        private InputReader _inputReader;
        [SerializeField]
        private Transform _transformTurret;

        private Vector2 _lastMousePosition;
        private Vector2 _worldMousePosition;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                return;
            }
            _player = GetComponent<Cat>();
            _inputReader.OnAimEvent += OnAim;
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner)
            {
                return;
            }
            _inputReader.OnAimEvent -= OnAim;
        }

        private void LateUpdate()
        {
            if (!IsOwner)
            {
                return;
            }
            _worldMousePosition = _player.GetCamera().ScreenToWorldPoint(_lastMousePosition);
            _transformTurret.up = new Vector2(_worldMousePosition.x - _transformTurret.position.x, _worldMousePosition.y - _transformTurret.position.y);
        }

        private void OnAim(Vector2 aimPosition)
        {
            if (aimPosition != _lastMousePosition)
            {
                _lastMousePosition = aimPosition;
            }
        }
    }
}