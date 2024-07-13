using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Unrez.Networking
{

    public class LobbyItem : MonoBehaviour
    {

        private Lobby _lobby;
        private LobbiesList _lobbiesList;
        [SerializeField]
        private TMP_Text _textLobbyName;
        [SerializeField]
        private TMP_Text _textLobbyPlayers;
        [SerializeField]
        private Button _buttonJoinLobby;

        
        private void Start()
        {
            _buttonJoinLobby.onClick.AddListener(Join);
        }

        private void Join()
        {
            _lobbiesList.JoinAsync(_lobby);
        }

        public void Initialize(LobbiesList lobbiesList, Lobby lobby)
        {
            _lobbiesList = lobbiesList;
            _lobby = lobby;
            _textLobbyName.text = _lobby.Name + $" [Time: {Time.time} ]";
            _textLobbyPlayers.text = _lobby.Players.Count + "/" + _lobby.MaxPlayers;
        }
    }
}