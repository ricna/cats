using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;


namespace Unrez
{
    public class Cat : NetworkBehaviour
    {
        private HealthController _healthController;
        private MotionController _motionController;

        [Header("References")]
        [SerializeField]
        private SpriteRenderer spriteRenderBody;
        [SerializeField]
        private CameraController cameraController;

        private Color[] _playerColorIDX = { Color.white, Color.gray, Color.cyan, Color.yellow, Color.blue, Color.magenta };
        private Color _myColor;

        private void Awake()
        {
            _healthController = GetComponent<HealthController>();
            _motionController = GetComponent<MotionController>();
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
            _healthController.TakeDamage(damage);
        }

        public Camera GetCamera()
        {
            return cameraController.GetCamera();
        }

        public void Dash()
        {
            spriteRenderBody.color = Color.red;
            _motionController.ApplyDash();
        }
        private IEnumerator Dashing()
        {
            while (_motionController.IsDashing())
            {
                yield return null;
            }
            spriteRenderBody.color = _myColor;
        }
        public bool IsDashing()
        {
            return _motionController.IsDashing();
        }

        public void CreateBarrier()
        {
            _motionController.CreateBarrier();
        }
    }
}