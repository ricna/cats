using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Unrez.Pets.Cats;

namespace Unrez.Netcode
{
    /// <summary>
    /// Spawn a differente Prefab as desired.
    /// Remove the Player Prefab from the NetworkManager to use it.
    /// After to Connect the OnNetworkSpawn will be called here, then we will going to Spawn the "Player Prefab".
    /// </summary>
    public class PlayerSpawner : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameObject _dog;
        [SerializeField]
        private GameObject _cat;
        [SerializeField]
        private Transform[] _spawnPoints;
        [SerializeField]
        private Transform _transformParent;

        [Header("Debugs")]
        [SerializeField]
        private Transform[] _spawnPointVariation;

        public override void OnNetworkSpawn()
        {

            if (IsServer)
            {
                SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, true);
            }
            else
            {
                SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, false);
            }
        }

        [ServerRpc(RequireOwnership = false)] //server owns this object but client can request a spawn
        public void SpawnPlayerServerRpc(ulong clientId, bool dog)
        {
            GameObject newPlayer;
            _spawnPointVariation = _spawnPoints[Random.Range(0, _spawnPoints.Length)].GetComponentsInChildren<Transform>();
            Debug.Log($"<color=black>SpawnPlayerServerRpc IsServer:{dog}</color>");
            if (dog)
            {
                newPlayer = Instantiate(_dog);
            }
            else
            {
                newPlayer = Instantiate(_cat);
            }

            newPlayer.transform.position = _spawnPointVariation[clientId + 1].position;
            NetworkObject networkObject = newPlayer.GetComponent<NetworkObject>();
            newPlayer.SetActive(true);
            networkObject.SpawnAsPlayerObject(clientId, true);
        }
    }
}