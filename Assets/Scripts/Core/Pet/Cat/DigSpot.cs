using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Unrez.Pets;
using Unrez.Pets.Cats;


namespace Unrez
{
    public class DigSpot : NetworkBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField]
        private int _id;

        [Header("Debugs")]
        [field: SerializeField]
        public BoneSpot BoneSpot { get; private set; }
        [SerializeField]
        private NetworkVariable<bool> _isAvailable = new NetworkVariable<bool>();
        [SerializeField]
        private Pet _petInteracting;
        private Collider2D _collider;
        public event Action<bool, bool> OnInteracting;

        private void Awake()
        {
            BoneSpot = GetComponentInParent<BoneSpot>();
            _collider = GetComponent<Collider2D>();
            if (BoneSpot == null)
            {
                Debug.LogError("DigSpot without a BoneSpot");
            }
            BoneSpot.OnBoneSpotDigged += OnSpotDigged;
            SetIsAvailableServerRpc(true);
        }

        public bool IsAvailable()
        {
            //aveilebol
            return _isAvailable.Value;
        }

        public void Interact(Pet pet)
        {
            if (!IsOwner)
            {
                return;
            }
            if (!_isAvailable.Value)
            {
                return;
            }
            _petInteracting = pet;
            InteractServerRpc(pet is Cat);

        }
        
        [ServerRpc]
        private void InteractServerRpc(bool isCat)
        {
            _isAvailable.Value = false;
            InteractClientRpc(isCat);
        }

        [ClientRpc]
        private void InteractClientRpc(bool isCat)
        {
            if (!IsOwner)
            {
                return;
            }
            OnInteracting?.Invoke(isCat, true);
        }

        public void Release()
        {
            if (!IsOwner)
            {
                return;
            }
            OnInteracting?.Invoke(_petInteracting is Cat, false);
            _petInteracting = null;
            //_isAvailable.Value = true;
            SetIsAvailableServerRpc(true);
        }

        [ServerRpc]
        private void SetIsAvailableServerRpc(bool available)
        {
            _isAvailable.Value = available;
        }

        public void OnSpotDigged(BoneSpot boneSpot)
        {
            if (!IsOwner)
            {
                return;
            }
            BoneSpotDiggedEventServerRpc();
        }

        [ServerRpc]
        private void BoneSpotDiggedEventServerRpc()
        {
            _isAvailable.Value = false;
            BoneSpotDiggedClientRpc();
        }

        [ClientRpc]
        private void BoneSpotDiggedClientRpc()
        {
            if (!IsOwner)
            {
                return;
            }
            Destroy(_collider);
        }
    }
}