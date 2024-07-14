using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unrez.Networking
{
    public class MainMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Button _buttonHost;
        [SerializeField]
        private Button _buttonJoin;
        [SerializeField]
        private Button _buttonLobbies;
        [SerializeField]
        private TMP_InputField _inputFieldJoinCode;
        [SerializeField]
        private CanvasGroup _canvasGroupLobbies;


        private void Start()
        {
            _buttonHost.onClick.AddListener(OnButtonStartHost);
            _buttonJoin.onClick.AddListener(OnButtonJoinServer);
            _buttonLobbies.onClick.AddListener(OnButtonShowLobbies);
        }

        public async void OnButtonStartHost()
        {
            await NetHandlerHost.Instance.StartHostAsync();
        }

        public async void OnButtonJoinServer()
        {
            await NetHandlerClient.Instance.JoinGameAsync(_inputFieldJoinCode.text);
        }

        public void OnButtonShowLobbies()
        {
            UnCanvas.ToggleCanvasGroup(_canvasGroupLobbies, true, 0.5f);
        }
    }
}