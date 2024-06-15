using System;
using Unity.Netcode;
using UnityEngine;
using Unrez.Pets;


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
        private bool _isAvailable = true;
        [SerializeField]
        private Pet _petInteracting;
        private Collider2D _collider;
        public event Action<Pet, BoneSpot, bool> OnInteracting;

        private void Awake()
        {
            BoneSpot = GetComponentInParent<BoneSpot>();
            _collider = GetComponent<Collider2D>();
            if (BoneSpot == null)
            {
                Debug.LogError("DigSpot without a BoneSpot");
            }
            BoneSpot.OnBoneSpotDigged += OnSpotDigged;
        }
        public bool IsAvailable()
        {
            //aveilebol
            return _isAvailable;
        }

        public void Interact(Pet pet)
        {
            if (!IsOwner)
            {
                return;
            }
            if (!_isAvailable)
            {
                return;
            }
            _isAvailable = false;
            _petInteracting = pet;
            OnInteracting?.Invoke(_petInteracting, BoneSpot, true);
        }
        
        public void Release()
        {
            OnInteracting?.Invoke(_petInteracting, BoneSpot, false);
            _petInteracting = null;
            _isAvailable = true;
        }

        public void OnSpotDigged(BoneSpot boneSpot)
        {
            _isAvailable = false;
            if (IsServer)
            {
                DestroyColliderServerRpc();
            }
        }

        [ServerRpc]
        private void DestroyColliderServerRpc()
        {
            DestroyColliderClientRpc();
        }
        [ClientRpc]
        private void DestroyColliderClientRpc()
        {
            Destroy(_collider);
        }
    }
}