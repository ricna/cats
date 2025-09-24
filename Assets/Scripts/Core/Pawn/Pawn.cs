using System;
using Unity.Netcode;
using UnityEngine;
using Unrez.Networking;


namespace Unrez.BackyardShowdown
{
    [Serializable]
    public struct PawnNetInfo
    {
        public ulong OwnerId;
        public string Name;
        public Color Color;
        public float Health;
        public float Speed;
    }

    public abstract class Pawn : NetworkBehaviour
    {
        [field: SerializeField]
        public PawnProfile Profile { get; private set; }

        protected Animator _animator;
        [SerializeField]
        protected Collider2D _collider;

        [SerializeField]
        private Transform _transformLightPosition;

        [SerializeField]
        protected PawnNetInfo _pawnNetInfo;
        protected PawnHealth _pawnHealth;
        protected PawnMotion _pawnMotion;
        protected PawnAbilities _pawnAbilities;

        // Referências cacheáveis para objetos da cena
        protected PawnCamera _pawnCamera;
        protected PawnMap _pawnMap;
        protected PawnLight _pawnLight;

        protected PawnTail _pawnTail;

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
            _pawnMotion = GetComponent<PawnMotion>();
            _pawnAbilities = GetComponent<PawnAbilities>();
            _pawnHealth = GetComponent<PawnHealth>();
            _pawnTail = GetComponentInChildren<PawnTail>();
            _collider = GetComponent<Collider2D>();
            _colliderOffset = _collider.offset;

            // Cachea as referências de objetos da cena em Awake, que é chamado antes de OnNetworkSpawn.
            _pawnMap = FindFirstObjectByType<PawnMap>();
            _pawnCamera = FindFirstObjectByType<PawnCamera>();
            _pawnLight = FindAnyObjectByType<PawnLight>();
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

            Profile = PawnsContainer.Instance.Pawns[OwnerClientId];

            if (playerSpawner.TestCatOnly)
            {
                if (this is Prey && OwnerClientId == 0)
                {
                    Profile = PawnsContainer.Instance.Pawns[PawnsContainer.Instance.Pawns.Length - 1];
                }
            }
            _pawnAbilities.Allocate(Profile.Abilities.Length);
            for (int i = 0; i < Profile.Abilities.Length; i++)
            {
                _pawnAbilities.SetAbility(i, Profile.Abilities[i]);
            }
            _pawnNetInfo = new PawnNetInfo();
            _pawnNetInfo.OwnerId = OwnerClientId;
            _pawnNetInfo.Color = Profile.Color;
            _pawnNetInfo.Name = $"[PET] {Profile.name}_00{OwnerClientId}";
            name = _pawnNetInfo.Name;
            _spriteRenderBody.color = _pawnNetInfo.Color;
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

            _pawnCamera.SetupCamera(gameObject, gameObject);
            _pawnLight.SetUp(this, Profile, _colliderOffset);
            _targetFOV = _currentFOV = Profile.PetView.OrthoSize;
            _pawnCamera.SetOrthoSize(_currentFOV);
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
                _pawnCamera.SetOrthoSize(_currentFOV);
                _pawnLight.SetRadius(lightRadius);
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

        public PawnTail GetTail()
        {
            return _pawnTail;
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

        public virtual PawnNetInfo GetStatus()
        {
            return _pawnNetInfo;
        }

        public virtual Camera GetCamera()
        {
            return _pawnCamera.GetCamera();
        }

        public abstract void TryAbility(int abilityId);
        public abstract void TakeHit(int damage);

        public virtual Color GetColor()
        {
            return _pawnNetInfo.Color;
        }

        public virtual Vector2 GetCurrentDirection()
        {
            return _pawnMotion.GetDirection();
        }

        public virtual Vector2 GetPointForward(float distance)
        {
            return new Vector2(GetCenter().x + (_pawnMotion.GetDirection().x * distance),
                GetCenter().y + (_pawnMotion.GetDirection().y) * distance);
        }

        public virtual bool IsMoving()
        {
            return _pawnMotion.IsMoving();
        }

        public virtual void ApplyImpulse(float impulse, float newLinearDrag = -1, bool useNewDirection = false, float newDirX = 0, float newDirY = 0)
        {
            _pawnMotion.ApplyImpulse(impulse, newLinearDrag, useNewDirection, newDirX, newDirY);
        }

        public virtual void SetMovementInput(Vector2 movementInput)
        {
            if (!CanMove())
            {
                return;
            }
            _pawnMotion.SetMovementInput(movementInput);
        }

        public virtual bool CanMove()
        {
            return !_pawnHealth.IsTakingHit();
        }

        public virtual void SetCrouchInput(bool pressing)
        {
            _pawnMotion.SetCrouchInput(pressing);
        }

        public virtual void SetSprintInput(bool pressing)
        {
            _pawnMotion.SetSprintInput(pressing);
        }

        public Ability GetAbilityByType(Type abilityType)
        {
            return _pawnAbilities.GetAbilityByType(abilityType);
        }

        public virtual void ToggleMinimap()
        {
            _minimapEnabled = !_minimapEnabled;
            if (_pawnMap != null)
            {
                _pawnMap.Display(_minimapEnabled);
            }
        }

        public abstract void ProcessInteractInput(bool pressing);

        public Transform GetLightPosition()
        {
            return _transformLightPosition;
        }
    }
}