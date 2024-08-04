using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Unrez.Networking;


namespace Unrez.BackyardShowdown
{
    [Serializable]
    public struct PetNetInfo
    {
        public ulong OwnerId;
        public string Name;
        public Color Color;
        public float Health;
        public float Speed;
    }

    public abstract class Pet : NetworkBehaviour
    {
        [field: SerializeField]
        public PetProfile Profile { get; private set; }

        [SerializeField]
        protected PetNetInfo _petNetInfo;
        protected PetHealth _healthController;
        protected PetMotion _motionController;
        protected PetAbilities _abilitiesController;
        protected Light2D _light;
        protected PetCamera _cameraController;
        protected Animator _animator;

        [Header("References")]
        [SerializeField]
        protected SpriteRenderer _spriteRenderBody;
        [SerializeField]
        private Transform _transformParent;

        [Header("Settings")]
        protected float _fovSpeed = 1;
        [SerializeField]
        private Vector2 _colliderOffset = new Vector2(0, 1.2f);

        [Header("Debug - FOV")]
        [SerializeField]
        private float _targetFOV;
        [SerializeField]
        private float _currentFOV;

        public event Action OnPetProfileLoaded;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _motionController = GetComponent<PetMotion>();
            _abilitiesController = GetComponent<PetAbilities>();
            _healthController = GetComponent<PetHealth>();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision) { }

        protected virtual void OnTriggerExit2D(Collider2D collision) { }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            SpawnPet();
            if (!IsOwner)
            {
                return;
            }
            InitializeLocalPet();
        }

        private void SpawnPet()
        {
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
            _petNetInfo = new PetNetInfo();
            _petNetInfo.OwnerId = OwnerClientId;
            _petNetInfo.Color = Profile.Color;
            _petNetInfo.Name = $"[PET] {Profile.name}_00{OwnerClientId}";
            //OnPetProfileLoaded?.Invoke(); //TODO - Share info with others? Abilities?
            name = _petNetInfo.Name;
            _spriteRenderBody.color = _petNetInfo.Color;
        }

        protected virtual void InitializeLocalPet()
        {
            if (!IsOwner)
            {
                return;
            }
            _cameraController = FindFirstObjectByType<PetCamera>();
            _cameraController.SetupCamera(gameObject, gameObject);
            _cameraController.SetOrthoSize(128, 0f);
            this.gameObject.AddComponent<AudioListener>();
            _light = (Light2D)FindAnyObjectByType(typeof(Light2D));
            _light.name = $"PetLight [{Profile.name}]";
            _light.enabled = true;
            _light.gameObject.transform.SetParent(transform);
            _light.gameObject.transform.localPosition = _colliderOffset;
            _light.lightType = Profile.PetView.LightType;
            _light.color = Profile.PetView.LightColor;
            _light.pointLightInnerRadius = Profile.PetView.LightRadius.x;
            _light.pointLightOuterRadius = Profile.PetView.LightRadius.y;
            _light.intensity = Profile.PetView.LightIntensity;
            _light.falloffIntensity = Profile.PetView.LightFalloffStrenght;
            _light.shadowsEnabled = Profile.PetView.Shadows;
            _light.shadowIntensity = Profile.PetView.ShadowsStrenght;
            _light.shadowSoftness = Profile.PetView.ShadowsSoftness;
            _light.shadowSoftnessFalloffIntensity = Profile.PetView.ShadowsFalloffStrenght;

            _cameraController.SetOrthoSize(Profile.PetView.OrthoSize, 3f);
        }

        protected virtual void Update()
        {
            if (!IsOwner)
            {
                return;
            }
            UpdateView();
        }

        protected void UpdateView()
        {
            if (!IsOwner)
            {
                return;
            }
            if (_currentFOV != _targetFOV)
            {
                _currentFOV = Mathf.Lerp(_currentFOV, _targetFOV, Time.deltaTime * _fovSpeed);
                _cameraController.SetOrthoSize(_currentFOV * 0.5f);
                _light.pointLightOuterRadius = _currentFOV;
                if (ChaseManager.Instance.ApplyChaseStatus)
                {
                    _cameraController.SetOrthoSize(_currentFOV * 0.5f);
                    _light.pointLightOuterRadius = _currentFOV;
                }
            }
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

        public virtual PetNetInfo GetStatus()
        {
            return _petNetInfo;
        }

        public virtual Camera GetCamera()
        {
            return _cameraController.GetCamera();
        }

        public abstract void TryAbility(int abilityId);
        public abstract void TakeHit(int damage);

        public virtual Color GetColor()
        {
            return _petNetInfo.Color;
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
            if (!CanMove())
            {
                return;
            }
            _motionController.SetMovementInput(movementInput);
        }

        public virtual bool CanMove()
        {
            if (_healthController.IsTakingHit())
            {
                return false;
            }
            return true;
        }

        public virtual void SetCrouchInput(bool pressing)
        {
            _motionController.SetCrouchInput(pressing);
        }

        public virtual void SetSprintInput(bool pressing)
        {
            _motionController.SetSprintInput(pressing);
        }

        public Ability GetAbilityByType(Type abilityType)
        {
            return _abilitiesController.GetAbilityByType(abilityType);
        }

        public abstract void ProcessInteractInput(bool pressing);


    }
}