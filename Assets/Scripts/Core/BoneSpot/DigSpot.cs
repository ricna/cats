using System;
using Unity.Netcode;
using UnityEngine;

namespace Unrez.BackyardShowdown
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

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            BoneSpot = GetComponentInParent<BoneSpot>();
            _collider = GetComponent<Collider2D>();
            if (BoneSpot == null)
            {
                Debug.LogError("DigSpot without a BoneSpot");
            }
            BoneSpot.OnBoneSpotDugUp += OnSpotDugUp;
            SetIsAvailableServerRpc(true);
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

        [ServerRpc(RequireOwnership = false)]
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
            if (_petInteracting != null)
            {
                OnInteracting?.Invoke(_petInteracting is Cat, false);
                _petInteracting = null;
                SetIsAvailableServerRpc(true);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetIsAvailableServerRpc(bool available)
        {
            _isAvailable.Value = available;
        }

        public void OnSpotDugUp(BoneSpot boneSpot)
        {
            if (!IsOwner)
            {
                return;
            }
            BoneSpotDugUpEventServerRpc();
        }

        [ServerRpc]
        private void BoneSpotDugUpEventServerRpc()
        {
            _isAvailable.Value = false;
            BoneSpotDugUpClientRpc();
        }

        [ClientRpc]
        private void BoneSpotDugUpClientRpc()
        {
            if (!IsOwner)
            {
                return;
            }
            Destroy(_collider);
        }

        public bool IsAvailable()
        {
            //aveilebol
            return _isAvailable.Value;
        }
    }
}