using System;
using Unity.Netcode;
using UnityEngine;
using Unrez.Pets;
using Unrez.Pets.Cats;
using Unrez.Pets.Dogs;

namespace Unrez.Networking
{
    /// <summary>
    /// Spawn a differente Prefab as desired.
    /// Remove the Player Prefab from the NetworkManager to use it.
    /// After to Connect the OnNetworkSpawn will be called here, then we will going to Spawn the "Player Prefab".
    /// </summary>
    public class PlayerSpawner : NetworkBehaviour
    {
        [Header("References")]
        [field: SerializeField]
        public bool TestCatOnly { get; private set; }
        [SerializeField]
        private GameObject _prefabDog;
        [SerializeField]
        private GameObject _prefabCat;
        [SerializeField]
        private Transform[] _spawnPoints;
        [SerializeField]
        private Transform _transformParent;

        [Header("Debugs")]
        [SerializeField]
        private Transform[] _spawnPointVariation;

        public event Action<ulong, Pet> OnPlayerSpawn;
        public event Action<ulong> OnPlayerDespawn;

        public override void OnNetworkSpawn()
        {
            if (TestCatOnly)
            {
                SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, false);
                return;
            }
            if (IsServer)
            {
                SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, true);
            }
            else
            {
                SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, false);
            }


        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            OnPlayerDespawn?.Invoke(OwnerClientId);
        }

        [ServerRpc(RequireOwnership = false)] //server owns this object but client can request a spawn
        public void SpawnPlayerServerRpc(ulong clientId, bool dog)
        {
            GameObject newPlayer;
            _spawnPointVariation = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)].GetComponentsInChildren<Transform>();
            Debug.Log($"<color=black>SpawnPlayerServerRpc IsServer:{dog}</color>");
            if (dog)
            {
                newPlayer = Instantiate(_prefabDog);
                ChaseManager.Instance.SetDog(newPlayer.GetComponent<Dog>());
            }
            else
            {
                newPlayer = Instantiate(_prefabCat);
                ChaseManager.Instance.AddCat(newPlayer.GetComponent<Cat>());
            }
            OnPlayerSpawn?.Invoke(OwnerClientId, newPlayer.GetComponent<Pet>());
            newPlayer.transform.position = _spawnPointVariation[clientId + 1].position;
            NetworkObject networkObject = newPlayer.GetComponent<NetworkObject>();
            newPlayer.SetActive(true);
            networkObject.SpawnAsPlayerObject(clientId, true);
        }
    }
}