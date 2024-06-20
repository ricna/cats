using System;
using Unity.IO.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Unrez.Netcode;
using Unrez.Pets.Abilities;
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
        public float Speed;
    }
    public enum PetViewStatus
    {
        Default,
        Chase,
        ChaseLevel02,
    }

    public abstract class Pet : NetworkBehaviour
    {
        [field: SerializeField]
        public PetProfile Profile { get; private set; }

        [SerializeField]
        protected PetStatus _petStatus;
        protected PetHealth _healthController;
        protected PetMotion _motionController;
        protected PetAbilities _abilitiesController;
        protected Light2D _light;
        protected PetCamera _cameraController;

        [Header("References")]
        [SerializeField]
        protected SpriteRenderer _spriteRenderBody;
        [SerializeField]
        private Transform _transformParent;

        public event Action OnPetProfileLoaded;

        [Header("Debug - FOV")]
        [SerializeField]
        private float _targetFOV;
        [SerializeField]
        private float _currentFOV;

        protected virtual void Awake()
        {
            _currentFOV = 48;
            _targetFOV = 36;
            _motionController = GetComponent<PetMotion>();
            _abilitiesController = GetComponent<PetAbilities>();
            _healthController = GetComponent<PetHealth>();
            _motionController.OnDirectionChangedEvent += OnDirectionChangedHandler;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Unbug.Log($"IsHost:{IsHost} IsOwner:{IsOwner} IsLocalPlayer:{IsLocalPlayer} NetworkBehaviourId:{NetworkBehaviourId} ", Uncolor.Black);
            Unbug.Log($"OwnerClientId:{OwnerClientId}", Uncolor.Red);
            PlayerSpawner playerSpawner = FindAnyObjectByType<PlayerSpawner>();
            Profile = PetsContainer.Instance.Pets[OwnerClientId];
            if (playerSpawner.TestCatOnly)
            {
                if (this is Cat && OwnerClientId == 0)
                {
                    Profile = PetsContainer.Instance.Pets[PetsContainer.Instance.Pets.Length - 1];
                }
            }
            _abilitiesController.Allocate(Profile.Abilities.Length);
            for (int i = 0; i < Profile.Abilities.Length; i++)
            {
                _abilitiesController.SetAbility(i, Profile.Abilities[i]);
            }
            _petStatus = new PetStatus();
            _petStatus.OwnerId = OwnerClientId;
            _petStatus.Color = Profile.Color;
            OnPetProfileLoaded?.Invoke();
            _petStatus.Name = $"[PET] {Profile.name}_00{OwnerClientId}";
            name = _petStatus.Name;
            _spriteRenderBody.color = _petStatus.Color;
            if (!IsOwner)
            {
                return;
            }
            InitializePet();
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        protected virtual void InitializePet()
        {
            if (!IsOwner)
            {
                return;
            }
            _cameraController = FindFirstObjectByType<PetCamera>();
            _cameraController.SetupCamera(gameObject, gameObject);
            this.gameObject.AddComponent<AudioListener>();

            _light = (Light2D)FindAnyObjectByType(typeof(Light2D));
            _light.name = $"PetLight [{Profile.name}]";
            _light.enabled = true;
            _light.gameObject.transform.SetParent(transform);
            _light.gameObject.transform.localPosition = Vector3.zero;

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

            _cameraController.SetOrthoSize(Profile.Light.CatView, 0.1f);
        }


        public virtual void SetFOV(float fov)
        {
            SetFOVClientRpc(fov);
        }

        [ClientRpc]
        private void SetFOVClientRpc(float fov)
        {
            if (!IsOwner)
            {
                return;
            }
            _targetFOV = fov;
        }

        protected void UpdateView()
        {
            if (!IsOwner)
            {
                return;
            }
            _light.pointLightOuterRadius = _currentFOV;
            _cameraController.SetOrthoSize(_currentFOV);
        }

        private float _fovSpeed = 1;
        protected virtual void Update()
        {
            if (!IsOwner)
            {
                return;
            }
            _currentFOV = Mathf.Lerp(_currentFOV, _targetFOV, Time.deltaTime * _fovSpeed);
            UpdateView();
        }

        protected virtual void OnDirectionChangedHandler(Vector2 vector)
        {
            //Debug.Log($"[{name}] -> Direction Changed {vector}");
        }

        public virtual PetStatus GetStatus()
        {
            return _petStatus;
        }

        public virtual Camera GetCamera()
        {
            return _cameraController.GetCamera();
        }

        public abstract void TryAbility(int abilityId);
        public abstract void TakeHit(int damage);

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

        public virtual void SetCrouchInput(bool pressing)
        {
        }

        public virtual void SetSprintInput(bool pressing)
        {
        }

        public Ability GetAbilityByType(Type abilityType)
        {
            return _abilitiesController.GetAbilityByType(abilityType);
        }

        //public abstract void OnDigSpotEnter();
        //public abstract void OnDigSpotExit();

        public abstract void ProcessInteractInput(bool pressing);


    }
}