using Unity.Netcode;
using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public class Map: NetworkBehaviour
    {
        [field: SerializeField]
        public BoneSpot[] BoneSpots { get; private set; }
        //private ScareTree[] _scareTree;

        private void Awake()
        {
            BoneSpots = FindObjectsByType(typeof(BoneSpot), FindObjectsSortMode.InstanceID) as BoneSpot[];
            //_scareTree = FindObjectsByType(typeof(ScareTree), FindObjectsSortMode.InstanceID) as ScareTree[];
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                // Inicialização do jogo, spawn de jogadores, etc.
                foreach (BoneSpot boneSpot in BoneSpots)
                {
                    if (boneSpot.IsSpawned)
                    {
                        Debug.Log($"BoneSpot Spawned {boneSpot.name}");
                    }
                    else
                    {
                        boneSpot.GetComponent<NetworkObject>().Spawn();
                    }
                    boneSpot.OnBoneSpotDugUp += BoneSpotDugHandle;
                }
            }
        }

        private void BoneSpotDugHandle(BoneSpot boneSpot)
        {
            Debug.Log("BoneSpot Dug!!!");
        }

    }
}