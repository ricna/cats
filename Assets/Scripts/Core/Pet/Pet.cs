using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Unrez.Pets.Cats;

namespace Unrez.Pets
{
    [Serializable]
    public struct PetStatus
    {
        public ulong OwnerId;
        public string Name;
        public Color Color;
        public float Health;
    }
    public abstract class Pet : NetworkBehaviour
    {
        [SerializeField]
        protected PetProfile _petProfile;
        [SerializeField]
        protected PetProfile[] _pets;

        protected PetStatus _petStatus;
        protected HealthController _healthController;
        protected MotionController _motionController;
        protected PerksController _perksController;
        protected Light2D _light;

        [Header("References")]
        [SerializeField]
        protected SpriteRenderer spriteRenderBody;
        [SerializeField]
        protected CameraController cameraController;
        [SerializeField]
        protected Color[] _playerColorIDX = { Color.cyan, Color.magenta, Color.white, Color.gray, Color.cyan, Color.yellow, Color.blue, Color.magenta };

        public event Action OnPetProfileLoaded;


        protected virtual void Awake()
        {
            InitLight();
            _healthController = GetComponent<HealthController>();
            _motionController = GetComponent<MotionController>();
            _perksController = GetComponent<PerksController>();
            _motionController.OnDirectionChangedEvent += OnDirectionChangedHandler;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Unbug.Log($"IsHost:{IsHost} IsOwner:{IsOwner} IsLocalPlayer:{IsLocalPlayer} NetworkBehaviourId:{NetworkBehaviourId} ", Uncolor.Black);
            Unbug.Log($"OwnerClientId:{OwnerClientId}", Uncolor.Red);
            _petStatus = new PetStatus();
            _petStatus.OwnerId = OwnerClientId;
            _petStatus.Color = _playerColorIDX[OwnerClientId];
            _petProfile = _pets[OwnerClientId];
            OnPetProfileLoaded?.Invoke();
            _petStatus.Name = $"Cat_00{OwnerClientId}";
            name = _petStatus.Name;
            spriteRenderBody.color = _petStatus.Color;
            if (!IsOwner)
            {
                return;
            }
            cameraController = FindFirstObjectByType<CameraController>();
            cameraController.SetupCamera(gameObject, gameObject);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        protected virtual void InitLight()
        {
            _light = (Light2D)FindAnyObjectByType(typeof(Light2D));
            _light.name = $"PetLight [{_petProfile.name}]";
            _light.enabled = true;
            _light.gameObject.transform.SetParent(transform);
            _light.gameObject.transform.position = Vector3.zero;

            _light.lightType = _petProfile.Light.LightType;
            _light.color = _petProfile.Light.LightColor;
            _light.pointLightInnerRadius = _petProfile.Light.LightRadius.x;
            _light.pointLightOuterRadius = _petProfile.Light.LightRadius.y;
            _light.intensity = _petProfile.Light.LightIntensity;
            _light.falloffIntensity = _petProfile.Light.LightFalloffStrenght;

            _light.shadowsEnabled = _petProfile.Light.Shadows;
            _light.shadowIntensity = _petProfile.Light.ShadowsStrenght;
            _light.shadowSoftness = _petProfile.Light.ShadowsSoftness;
            _light.shadowSoftnessFalloffIntensity = _petProfile.Light.ShadowsFalloffStrenght;
        }

        protected virtual void OnDirectionChangedHandler(Vector2 vector)
        {
            _perksController.UpdateSpawnBehindPosition(vector);
        }

        public virtual PetProfile GetProfile()
        {
            return _petProfile;
        }

        public virtual PetStatus GetStatus()
        {
            return _petStatus;
        }

        public virtual Camera GetCamera()
        {
            return cameraController.GetCamera();
        }

        public abstract void TryAbility01();
        public abstract void TryAbility02();
        public abstract void TryAbility03();
        public abstract void TryAbility04();
        public abstract void TakeDamage(int damage);

        public virtual Color GetColor()
        {
            return _petStatus.Color;
        }

        public virtual Vector2 GetCurrentDirection()
        {
            return _motionController.GetCurrentDirection();
        }

        public virtual bool IsMoving()
        {
            return _motionController.IsMoving();
        }

        public virtual void ApplyImpulse(float impulse, float newLinearDrag = -1, bool useNewDirection = false, float newDirX = 0, float newDirY = 0)
        {
            _motionController.ApplyImpulse(impulse, newLinearDrag, useNewDirection, newDirX, newDirY);
        }

        public virtual void SetMovementInput(Vector2 movementInput)
        {
            _motionController.SetMovementInput(movementInput);
        }



    }


}