using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Unrez.Cats
{
    [Serializable]
    public struct CatStatus
    {
        public ulong OwnerId;
        public string Name;
        public Color Color;
        public float Health;
    }
    public class Cat : NetworkBehaviour
    {
        private Light2D _light;
        private CatStatus _catStatus;
        private HealthController _healthController;
        private MotionController _motionController;
        private PerksController _perksController;
        [Header("References")]
        [SerializeField]
        private SpriteRenderer spriteRenderBody;
        [SerializeField]
        private CameraController cameraController;
        [SerializeField]
        private Color[] _playerColorIDX = { Color.cyan, Color.magenta, Color.white, Color.gray, Color.cyan, Color.yellow, Color.blue, Color.magenta };
        private void Awake()
        {
            _light = (Light2D)FindAnyObjectByType(typeof(Light2D));
            _light.gameObject.transform.SetParent(transform);
            _light.gameObject.transform.position = Vector3.zero;
            _healthController = GetComponent<HealthController>();
            _motionController = GetComponent<MotionController>();
            _perksController = GetComponent<PerksController>();
            _motionController.OnDirectionChangedEvent += OnDirectionChangedHandler;
        }
        private void OnDirectionChangedHandler(Vector2 vector)
        {
            _perksController.UpdateSpawnBehindPosition(vector);
        }

        public override void OnNetworkSpawn()
        {
            Unbug.Log($"IsHost:{IsHost} IsOwner:{IsOwner} IsLocalPlayer:{IsLocalPlayer} NetworkBehaviourId:{NetworkBehaviourId} ", Uncolor.Black);
            Unbug.Log($"OwnerClientId:{OwnerClientId}", Uncolor.Red);
            _catStatus = new CatStatus();
            _catStatus.OwnerId = OwnerClientId;
            _catStatus.Color = _playerColorIDX[OwnerClientId];
            _catStatus.Name = $"Cat_00{OwnerClientId}";
            name = _catStatus.Name;
            spriteRenderBody.color = _catStatus.Color;
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

        internal void Dash()
        {
            if (_perksController.CanDash())
            {
                spriteRenderBody.color = Color.red;
                _perksController.ApplyDash();
                StartCoroutine(Dashing());
            }
        }

        private IEnumerator Dashing()
        {
            while (_perksController.IsDashing())
            {
                yield return null;
            }
            spriteRenderBody.color = _catStatus.Color;
        }

        public bool IsDashing()
        {
            return _perksController.IsDashing();
        }

        public void CreateBarrier()
        {
            _perksController.CreateBarrier();
        }

        public Color GetColor()
        {
            return _catStatus.Color;
        }

        public CatStatus GetData()
        {
            return _catStatus;
        }

        public Vector2 GetCurrentDirection()
        {
            return _motionController.GetCurrentDirection();
        }

        public bool IsMoving()
        {
            return _motionController.IsMoving();
        }


        public void ApplyImpulse(float impulse, float newLinearDrag = -1, bool useNewDirection = false, float newDirX = 0, float newDirY = 0)
        {
            _motionController.ApplyImpulse(impulse, newLinearDrag, useNewDirection, newDirX, newDirY);
        }

        public void SetMovementInput(Vector2 movementInput)
        {
            _motionController.SetMovementInput(movementInput);
        }
    }
}