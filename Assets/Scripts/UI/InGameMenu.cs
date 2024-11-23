using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unrez.Networking;

namespace Unrez.BackyardShowdown
{
    public class InGameMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Button _buttonLeaveMatch;
        [SerializeField]
        private Button _buttonSettings;


        private void Start()
        {
            _buttonLeaveMatch.onClick.AddListener(OnButtonLeaveMatch);
            _buttonSettings.onClick.AddListener(OnButtonSettings);
        }

        public void OnButtonLeaveMatch()
        {
            LeaveMatch();
        }

        public async void OnButtonSettings()
        {
            
        }

        public void LeaveMatch()
        {
            // If the local player is the host
            if (NetworkManager.Singleton.IsHost)
            {
                Debug.Log("Host is shutting down the server...");
                NetworkManager.Singleton.Shutdown(); // Shutdown the server and disconnect all clients
            }
            // If the local player is a client
            else if (NetworkManager.Singleton.IsClient)
            {
                Debug.Log("Client is disconnecting...");
                NetworkManager.Singleton.Shutdown(); // Disconnect the client
            }
            else
            {
                Debug.LogWarning("Not connected to a match.");
            }

            // Clean up any game-specific data if necessary
            CleanupAfterLeave();

            // Optionally return to a menu scene
            LoadMainMenu();
        }

        private void CleanupAfterLeave()
        {
            // Example: Clear any local data or UI related to the match
            Debug.Log("Cleaning up after leaving the match...");
        }

        private void LoadMainMenu()
        {
            Debug.Log("Returning to the main menu...");
            // Load the main menu scene if you have one
            // Replace "MainMenu" with your actual scene name
            NetworkManager.Singleton.SceneManager.LoadScene(NetworkingScenes.SCENE_MAIN_MENU, LoadSceneMode.Single);
        }
    }
}
