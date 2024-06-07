using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;


namespace Unrez
{

    [Serializable]
    public struct CatData
    {
        public ulong OwnerId;
        public string Name;
        public Color Color;
    }

    public class Cat : NetworkBehaviour
    {
        private CatData _data;
        private HealthController _healthController;
        private MotionController _motionController;

        [Header("References")]
        [SerializeField]
        private SpriteRenderer spriteRenderBody;
        [SerializeField]
        private CameraController cameraController;
        [SerializeField]
        private Color[] _playerColorIDX = { Color.cyan, Color.magenta, Color.white, Color.gray, Color.cyan, Color.yellow, Color.blue, Color.magenta };

        private void Awake()
        {
            _healthController = GetComponent<HealthController>();
            _motionController = GetComponent<MotionController>();
        }

        public override void OnNetworkSpawn()
        {
            Unbug.Log($"IsHost:{IsHost} IsOwner:{IsOwner} IsLocalPlayer:{IsLocalPlayer} NetworkBehaviourId:{NetworkBehaviourId} ", Uncolor.Black);
            Unbug.Log($"OwnerClientId:{OwnerClientId}", Uncolor.Red);
            _data = new CatData();
            _data.OwnerId = OwnerClientId;
            _data.Color = _playerColorIDX[OwnerClientId];
            _data.Name = $"Cat_00{OwnerClientId}";
            name = _data.Name;
            spriteRenderBody.color = _data.Color;
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
            if (_motionController.CanDash())
            {
                spriteRenderBody.color = Color.red;
                _motionController.ApplyDash();
                StartCoroutine(Dashing());
            }
        }

        private IEnumerator Dashing()
        {
            while (_motionController.IsDashing())
            {
                yield return null;
            }
            spriteRenderBody.color = _data.Color;
        }
        public bool IsDashing()
        {
            return _motionController.IsDashing();
        }

        public void CreateBarrier()
        {
            _motionController.CreateBarrier();
        }

        public Color GetColor()
        {
            return _data.Color;
        }

        public CatData GetData()
        {
            return _data;
        }
    }
}