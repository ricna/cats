using System;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Unrez.Networking
{
    public class LobbiesList : MonoBehaviour
    {

        [Header("References")]
        [SerializeField]
        private LobbyItem _lobbyItemPrefab;
        [SerializeField]
        private Transform _lobbyItemParent;
        [SerializeField]
        private Button _buttonRefresh;
        [SerializeField]
        private Button _buttonQuit;
        [SerializeField]
        private CanvasGroup _canvasGroupLobbies;

        private bool _isJoining = false;
        private bool _isRefreshing = false;

        private void OnEnable()
        {
            _buttonRefresh.onClick.AddListener(Refresh);
            _buttonQuit.onClick.AddListener(Quit);
            Refresh();
        }

        private void OnDisable()
        {
            _buttonRefresh.onClick.RemoveListener(Refresh);
            _buttonQuit.onClick.RemoveListener(Quit);
        }

        private void Quit()
        {
            UnCanvas.ToggleCanvasGroup(_canvasGroupLobbies);
        }

        public async void JoinAsync(Lobby lobby)
        {
            if (_isJoining)
            {
                return;
            }
            _isJoining = true;
            try
            {
                Lobby joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
                string joinCode = joiningLobby.Data["JoinCode"].Value;
                await ClientSingleton.Instance.JoinGameAsync(joinCode);
            }
            catch (LobbyServiceException ex)
            {
                Debug.LogException(ex);
            }
            _isJoining = false;
        }

        private async void Refresh()
        {
            
            if (_isRefreshing)
            {
                Debug.Log("Already Refreshing");
                return;
            }
            _isRefreshing = true;
            try
            {
                Debug.Log("Start Refreshing");
                foreach (Transform child in _lobbyItemParent)
                {
                    Destroy(child.gameObject);
                }
                QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions();
                queryLobbiesOptions.Count = 10;
                queryLobbiesOptions.Filters = new List<QueryFilter>()
                {
                    new QueryFilter(
                        field:QueryFilter.FieldOptions.AvailableSlots,//all available slots/lobbies
                        op: QueryFilter.OpOptions.GT,//make sure they are Greather Than GT
                        value:"0" //Greather Than ZERO
                    ),
                    new QueryFilter(
                        field:QueryFilter.FieldOptions.IsLocked,  //hide locked rooms (only show locked lobbies when Equal ZERO, which means... none)
                        op: QueryFilter.OpOptions.EQ,
                        value:"0"
                        //Combining EQ and value=0 in the QueryFilter, is the way to hide a 'field' in a query
                    ),
                };
                queryLobbiesOptions.Order = new List<QueryOrder>() { new QueryOrder(asc: false, field: QueryOrder.FieldOptions.Created) };
                QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
                foreach (Lobby lobby in lobbies.Results)
                {
                    LobbyItem lobbyItem = Instantiate(_lobbyItemPrefab, _lobbyItemParent);
                    lobbyItem.Initialize(this, lobby);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            _isRefreshing = false;
        }
    }
}