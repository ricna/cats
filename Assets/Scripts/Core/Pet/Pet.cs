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
        [field: SerializeField]
        public PetProfile Profile { get; private set; }

        protected PetStatus _petStatus;
        protected PetHealth _healthController;
        protected PetMotion _motionController;
        protected PetAbility _abilityController;
        protected Light2D _light;
        protected PetCamera _cameraController;

        [Header("References")]
        [SerializeField]
        protected SpriteRenderer _spriteRenderBody;

        public event Action OnPetProfileLoaded;

        protected virtual void Awake()
        {
            _motionController = GetComponent<PetMotion>();
            _abilityController = GetComponent<PetAbility>();
            _healthController = GetComponent<PetHealth>();
            _motionController.OnDirectionChangedEvent += OnDirectionChangedHandler;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Unbug.Log($"IsHost:{IsHost} IsOwner:{IsOwner} IsLocalPlayer:{IsLocalPlayer} NetworkBehaviourId:{NetworkBehaviourId} ", Uncolor.Black);
            Unbug.Log($"OwnerClientId:{OwnerClientId}", Uncolor.Red);
            Profile = PetsContainer.Instance.Pets[OwnerClientId];
            _petStatus = new PetStatus();
            _petStatus.OwnerId = OwnerClientId;
            _petStatus.Color = Profile.Color;
            OnPetProfileLoaded?.Invoke();
            _petStatus.Name = $"Cat_00{OwnerClientId}";
            name = _petStatus.Name;
            _spriteRenderBody.color = _petStatus.Color;
            if (!IsOwner)
            {
                return;
            }
            _cameraController = FindFirstObjectByType<PetCamera>();
            _cameraController.SetupCamera(gameObject, gameObject);
            InitLight();
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

        }

        protected virtual void InitLight()
        {
            _light = (Light2D)FindAnyObjectByType(typeof(Light2D));
            _light.name = $"PetLight [{Profile.name}]";
            _light.enabled = true;
            _light.gameObject.transform.SetParent(transform);
            _light.gameObject.transform.position = Vector3.zero;

            _light.lightType = Profile.Light.LightType;
            _light.color = Profile.Light.LightColor;
            _light.pointLightInnerRadius = Profile.Light.LightRadius.x;
            _light.pointLightOuterRadius = Profile.Light.LightRadius.y;
            _light.intensity = Profile.Light.LightIntensity;
            _light.falloffIntensity = Profile.Light.LightFalloffStrenght;

            _light.shadowsEnabled = Profile.Light.Shadows;
            _light.shadowIntensity = Profile.Light.ShadowsStrenght;
            _light.shadowSoftness = Profile.Light.ShadowsSoftness;
            _light.shadowSoftnessFalloffIntensity = Profile.Light.ShadowsFalloffStrenght;
        }

        protected virtual void OnDirectionChangedHandler(Vector2 vector)
        {
            _abilityController.UpdateSpawnBehindPosition(vector);
        }

        public virtual PetProfile GetProfile()
        {
            return Profile;
        }

        public virtual PetStatus GetStatus()
        {
            return _petStatus;
        }

        public virtual Camera GetCamera()
        {
            return _cameraController.GetCamera();
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