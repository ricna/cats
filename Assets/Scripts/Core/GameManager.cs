using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Unrez.Netcode;
using Unrez.Pets;
using Unrez.Pets.Cats;
using Unrez.Pets.Dogs;


namespace Unrez
{

    public class GameManager : NetworkBehaviour
    {
        public Map _map;
        public List<Cat> _cats;
        public Dog _dog;
        private PlayerSpawner _playerSpawner;

        private void Awake()
        {
            _playerSpawner = GetComponent<PlayerSpawner>();
            _playerSpawner.OnPlayerSpawn += OnPlayerSpawnHandler;
        }

        private void OnPlayerSpawnHandler(ulong ownerId, Pet pet)
        {
            if (pet is Cat cat)
            {
                Debug.Log($"OnPlayerSpawnHandler - OwnerId {ownerId} Cat{cat.name}");
            }
            else
            {
                Debug.Log($"OnPlayerSpawnHandler - OwnerId {ownerId} DOG{pet.name}");
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                // Inicialização do jogo, spawn de jogadores, etc.
                foreach (BoneSpot boneSpot in _map.BoneSpots)
                {
                    boneSpot.OnBoneSpotDigged += BoneSpotDiggedHandle;
                }
            }
        }

        private void BoneSpotDiggedHandle(BoneSpot boneSpot)
        {
            Debug.Log("BoneSpot Digged!!!");
        }
    }

}