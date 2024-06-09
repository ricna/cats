using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;

namespace Unrez
{
    public class NetworkManager4Pets : NetworkManager
    {
        /*
        private GameObject[] Pets;
        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            GameObject player;

            if (chosenClass == CharacterClasses.Ronin)
                player = Instantiate(Resources.Load("Ronin Prefab"), transform.position, Quaternion.identity) as GameObject;

            if (chosenClass == CharacterClasses.Samurai)
                player = Instantiate(Resources.Load("Samurai Prefab"), transform.position, Quaternion.identity) as GameObject;

            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }*/
    }
}