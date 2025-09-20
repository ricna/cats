using System;
using Unity.Netcode;
using UnityEngine;
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

        protected Animator _animator;
        [SerializeField]
        protected Collider2D _collider;

        [SerializeField]
        protected PetNetInfo _petNetInfo;
        protected PetHealth _petHealth;
        protected PetMotion _petMotion;
        protected PetAbilities _petAbilities;

        // Referências cacheáveis para objetos da cena
        protected PetCamera _petCamera;
        protected PetMap _petMap;
        protected PetLight _petLight;

        protected PetTail _petTail;

        protected bool _minimapEnabled = false;

        [Header("References")]
        [SerializeField]
        protected SpriteRenderer _spriteRenderBody;
        [SerializeField]
        private Transform _transformParent;

        [Header("Settings")]
        protected float _fovSpeed = 1;
        [SerializeField]
        private float _tailOffset;
        private Vector2 _colliderOffset;
        [SerializeField]
        private float _ratioFOVLightInner = 4;
        [SerializeField]
        private float _ratioFOVLightOuter = 8;

        [Header("Debug - FOV")]
        [SerializeField]
        private float _targetFOV;
        [SerializeField]
        private float _currentFOV;

        private int _idxPet = 0;

        public event Action OnPetProfileLoaded;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _petMotion = GetComponent<PetMotion>();
            _petAbilities = GetComponent<PetAbilities>();
            _petHealth = GetComponent<PetHealth>();
            _petTail = GetComponentInChildren<PetTail>();
            _collider = GetComponent<Collider2D>();
            _colliderOffset = _collider.offset;

            // Cachea as referências de objetos da cena em Awake, que é chamado antes de OnNetworkSpawn.
            _petMap = FindFirstObjectByType<PetMap>();
            _petCamera = FindFirstObjectByType<PetCamera>();
            _petLight = FindAnyObjectByType<PetLight>();
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
            Debug.Log($"IsHost:{IsHost} IsOwner:{IsOwner} IsLocalPlayer:{IsLocalPlayer} NetworkBehaviourId:{NetworkBehaviourId} ");
            Debug.Log($"OwnerClientId:{OwnerClientId}");
            PlayerSpawner playerSpawner = FindAnyObjectByType<PlayerSpawner>();

            Profile = PetsContainer.Instance.Pets[OwnerClientId];

            if (playerSpawner.TestCatOnly)
            {
                if (this is Cat && OwnerClientId == 0)
                {
                    Profile = PetsContainer.Instance.Pets[PetsContainer.Instance.Pets.Length - 1];
                }
            }
            _petAbilities.Allocate(Profile.Abilities.Length);
            for (int i = 0; i < Profile.Abilities.Length; i++)
            {
                _petAbilities.SetAbility(i, Profile.Abilities[i]);
            }
            _petNetInfo = new PetNetInfo();
            _petNetInfo.OwnerId = OwnerClientId;
            _petNetInfo.Color = Profile.Color;
            _petNetInfo.Name = $"[PET] {Profile.name}_00{OwnerClientId}";
            name = _petNetInfo.Name;
            _spriteRenderBody.color = _petNetInfo.Color;
        }

        protected virtual void InitializeLocalPet()
        {
            if (!IsOwner)
            {
                return;
            }
            this.gameObject.AddComponent<AudioListener>();

            _minimapEnabled = false;
            ToggleMinimap();

            _petCamera.SetupCamera(gameObject, gameObject);
            _petLight.SetUp(this, Profile, _colliderOffset);

            _targetFOV = _currentFOV = Profile.PetView.OrthoSize;
            _petCamera.SetOrthoSize(_currentFOV);
        }

        protected virtual void Update()
        {
            if (!IsOwner)
            {
                return;
            }
            UpdateView();
        }

        protected virtual void UpdateView()
        {
            if (!IsOwner)
            {
                return;
            }
            if (_currentFOV != _targetFOV)
            {
                _currentFOV = Mathf.Lerp(_currentFOV, _targetFOV, Time.deltaTime * _fovSpeed);
                if (Mathf.Abs(_currentFOV - _targetFOV) < 0.01)
                {
                    _currentFOV = _targetFOV;
                }

                Vector2 lightRadius = new Vector2(_currentFOV * _ratioFOVLightInner, _currentFOV * _ratioFOVLightOuter);
                _petCamera.SetOrthoSize(_currentFOV);
                _petLight.SetRadius(lightRadius);
            }
        }

        public virtual bool CanSprint()
        {
            return true; // Padrão: qualquer pet pode correr
        }

        public virtual bool CanCrouch()
        {
            return true; // Padrão: qualquer pet pode agachar
        }

        public PetTail GetTail()
        {
            return _petTail;
        }

        public Vector2 GetCenter()
        {
            return _collider.bounds.center;
        }

        public virtual void SetFOV(float fov)
        {
            if (!IsOwner) return;
            _targetFOV = fov;
            SetFOVClientRpc(fov);
        }

        [ClientRpc]
        private void SetFOVClientRpc(float fov)
        {
            if (IsOwner)
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
            return _petCamera.GetCamera();
        }

        public abstract void TryAbility(int abilityId);
        public abstract void TakeHit(int damage);

        public virtual Color GetColor()
        {
            return _petNetInfo.Color;
        }

        public virtual Vector2 GetCurrentDirection()
        {
            return _petMotion.GetDirection();
        }

        public virtual Vector2 GetPointForward(float distance)
        {
            return new Vector2(GetCenter().x + (_petMotion.GetDirection().x * distance),
                GetCenter().y + (_petMotion.GetDirection().y) * distance);
        }

        public virtual bool IsMoving()
        {
            return _petMotion.IsMoving();
        }

        public virtual void ApplyImpulse(float impulse, float newLinearDrag = -1, bool useNewDirection = false, float newDirX = 0, float newDirY = 0)
        {
            _petMotion.ApplyImpulse(impulse, newLinearDrag, useNewDirection, newDirX, newDirY);
        }

        public virtual void SetMovementInput(Vector2 movementInput)
        {
            if (!CanMove())
            {
                return;
            }
            _petMotion.SetMovementInput(movementInput);
        }

        public virtual bool CanMove()
        {
            return !_petHealth.IsTakingHit();
        }

        public virtual void SetCrouchInput(bool pressing)
        {
            _petMotion.SetCrouchInput(pressing);
        }

        public virtual void SetSprintInput(bool pressing)
        {
            _petMotion.SetSprintInput(pressing);
        }

        public Ability GetAbilityByType(Type abilityType)
        {
            return _petAbilities.GetAbilityByType(abilityType);
        }

        public virtual void ToggleMinimap()
        {
            _minimapEnabled = !_minimapEnabled;
            if (_petMap != null)
            {
                _petMap.Display(_minimapEnabled);
            }
        }

        public abstract void ProcessInteractInput(bool pressing);
    }
}