using System.Collections.Generic;
using Unity.Netcode;
using Unrez.Netcode;
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
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                // Inicializa��o do jogo, spawn de jogadores, etc.
                foreach (BoneSpot boneSpot in _map.BoneSpots)
                {
                    boneSpot.OnBoneSpotDigged += HandleBoneSpotDigged;
                }
            }
        }

        private void HandleBoneSpotDigged(BoneSpot boneSpot)
        {
            // L�gica para quando um gerador � reparado no servidor
        }
    }

}