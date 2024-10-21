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
        protected Collider2D _collider;

        [SerializeField]
        protected PetNetInfo _petNetInfo;
        protected PetHealth _petHealth;
        protected PetMotion _petMotion;
        protected PetAbilities _petAbilities;
        protected PetCamera _petCamera;
        protected PetMap _petMap;
        protected PetLight _petLight;
        protected bool _minimapEnabled = false;

        [Header("References")]
        [SerializeField]
        protected SpriteRenderer _spriteRenderBody;
        [SerializeField]
        private Transform _transformParent;

        [Header("Settings")]
        protected float _fovSpeed = 1;
        [SerializeField]
        private Vector2 _colliderOffset = new Vector2(0, 1.2f);
        [SerializeField]
        private float _ratioFOVLightInner = 4;
        [SerializeField]
        private float _ratioFOVLightOuter = 8;

        [Header("Debug - FOV")]
        [SerializeField]
        private float _targetFOV;
        [SerializeField]
        private float _currentFOV;


        public event Action OnPetProfileLoaded;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _petMotion = GetComponent<PetMotion>();
            _petAbilities = GetComponent<PetAbilities>();
            _petHealth = GetComponent<PetHealth>();
            _collider = GetComponent<Collider2D>();
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
            _petAbilities.Allocate(Profile.Abilities.Length);
            for (int i = 0; i < Profile.Abilities.Length; i++)
            {
                _petAbilities.SetAbility(i, Profile.Abilities[i]);
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
            //Audio
            this.gameObject.AddComponent<AudioListener>();

            //Map
            _petMap = FindFirstObjectByType<PetMap>();
            _minimapEnabled = false;
            ToggleMinimap();

            //Camera
            _petCamera = FindFirstObjectByType<PetCamera>();
            _petCamera.SetupCamera(gameObject, gameObject);
            //_petCamera.SetOrthoSize(64, 0f);
            //_currentFOV = 64;

            //Light
            _petLight = (PetLight)FindAnyObjectByType(typeof(PetLight));
            _petLight.SetUp(this, Profile, _colliderOffset);

            //Start FOV
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
                _petCamera.SetOrthoSize(_currentFOV);
                if (this is Cat)
                {
                    _petLight.SetRadius(new Vector2(_currentFOV * _ratioFOVLightInner, _currentFOV * _ratioFOVLightOuter));
                }
                else
                {
                    _petLight.SetRadius(new Vector2(_currentFOV * _ratioFOVLightInner, _currentFOV * _ratioFOVLightOuter));
                }

                /*if (ChaseManager.Instance.ApplyChaseStatus)
                {
                    _cameraController.SetOrthoSize(_currentFOV);
                    _light.pointLightOuterRadius = _currentFOV * 4;
                }*/
            }
        }

        public Vector2 GetCenter()
        {
            return _collider.bounds.center;
        }

        public virtual void SetFOV(float fov)
        {
            SetFOVClientRpc(fov);
        }

        [ClientRpc]
        private void SetFOVClientRpc(float fov)// security? why ClientRpc? A: Came from ChaseManager and its only server script ;)
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
            if (_petHealth.IsTakingHit())
            {
                return false;
            }
            return true;
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
            if (_minimapEnabled)
            {
                _petMap.Display(true);
            }
            else
            {
                _petMap.Display(false);
            }
        }

        public abstract void ProcessInteractInput(bool pressing);

        #region Flip

        public virtual void Flip(PetSide petSide)
        {
            FlipFinally(petSide);
            FlipServerRpc(petSide);
        }

        [ServerRpc(RequireOwnership = false)]
        public virtual void FlipServerRpc(PetSide petSide)
        {
            //Debug.Log($"<color=blue>FlipServerRpc by {name}</color>");
            FlipClientRpc(petSide);
        }

        [ClientRpc]
        public virtual void FlipClientRpc(PetSide petSide)
        {
            if (IsOwner)
            {
                return;
            }
            FlipFinally(petSide);
        }

        public virtual void FlipFinally(PetSide petSide)
        {
            switch (petSide)
            {
                case PetSide.West:
                case PetSide.SouthWest:
                case PetSide.NorthWest:
                    _spriteRenderBody.flipX = true;
                    break;
                default:
                case PetSide.East:
                case PetSide.NorthEast:
                case PetSide.SouthEast:
                case PetSide.North:
                case PetSide.South:
                    _spriteRenderBody.flipX = false;
                    break;
            }
        }
        #endregion
    }
}