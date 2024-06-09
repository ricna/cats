using Unity.Netcode;
using UnityEngine;
using Unrez.Pets.Cats;

namespace Unrez.Pets
{
    public class PetSpawnSelector : NetworkBehaviour
    {
        [SerializeField]
        private GameObject _dog;
        [SerializeField]
        private GameObject _cat;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            PetProfile petProfile = PetsContainer.Instance.Pets[OwnerClientId];
            SetPetServerRpc(petProfile.PetType);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPetServerRpc(PetType petType)
        {
            if (!IsOwner)
            {
                return;
            }
            switch (petType)
            {
                case PetType.Cat:
                    Destroy(_dog);
                    _cat.SetActive(true);
                    _cat.transform.SetParent(null);
                    break;
                case PetType.Dog:
                    Destroy(_cat);
                    _dog.SetActive(true);
                    _dog.transform.SetParent(null);
                    break;
            }
            Destroy(this.gameObject);
        }
    }
}