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
        private NetworkVariable<bool> _isAvailable = new NetworkVariable<bool>();
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
            return _isAvailable.Value;
        }

        public void Interact(Pet pet)
        {
            if (!IsServer)
            {
                return;
            }
            if (!_isAvailable.Value)
            {
                return;
            }

            _isAvailable.Value = false;
            _petInteracting = pet;
            OnInteracting?.Invoke(_petInteracting, BoneSpot, true);
        }

        public void Release()
        {
            OnInteracting?.Invoke(_petInteracting, BoneSpot, false);
            _petInteracting = null;
            _isAvailable.Value = true;
        }

        public void OnSpotDigged(BoneSpot boneSpot)
        {
            if (IsServer)
            {
                BoneSpotDiggedEventServerRpc();
            }
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
            Destroy(_collider);
        }
    }
}