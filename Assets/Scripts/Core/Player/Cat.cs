using Unity.Netcode;
using UnityEngine;


namespace Unrez
{
    public class Cat : NetworkBehaviour
    {
        private HealthController healthController;
        private MotionController motionController;

        [Header("References")]
        [SerializeField]
        private SpriteRenderer spriteRenderBody;
        [SerializeField]
        private CameraController cameraController;

        private Color[] _playerColorIDX = { Color.white, Color.red, Color.green, Color.yellow, Color.cyan, Color.magenta };
        private Color _myColor;

        private void Awake()
        {
            healthController = GetComponent<HealthController>();
            motionController = GetComponent<MotionController>();
        }

        public override void OnNetworkSpawn()
        {
            Unbug.Log($"IsHost:{IsHost} IsOwner:{IsOwner} IsLocalPlayer:{IsLocalPlayer} NetworkBehaviourId:{NetworkBehaviourId} ", Uncolor.Black);
            Unbug.Log($"OwnerClientId:{OwnerClientId}", Uncolor.Red);

            _myColor = _playerColorIDX[OwnerClientId];

            spriteRenderBody.color = _myColor;
            if (!IsOwner)
            {
                return;
            }
            cameraController = FindFirstObjectByType<CameraController>();
            cameraController.SetupCamera(gameObject, gameObject);
        }

        public void TakeDamage(int damage)
        {
            healthController.TakeDamage(damage);
        }

        public Camera GetCamera()
        {
            return cameraController.GetCamera();
        }
    }
}