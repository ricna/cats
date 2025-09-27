using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unrez.Networking;
using System;

namespace Unrez.BackyardShowdown
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("A��es de Conex�o")]
        [SerializeField]
        private Button _buttonHost;
        [SerializeField]
        private Button _buttonJoin;
        [SerializeField]
        private TMP_InputField _inputFieldJoinCode;

        [Header("Navega��o de UI")]
        [SerializeField]
        private Button _buttonLobbies;
        [SerializeField]
        private CanvasGroup _canvasGroupLobbies;
        [SerializeField]
        private Button _buttonSettings;
        [SerializeField]
        private Button _buttonQuit;

        [Header("Refer�ncias de Pain�is")]
        [SerializeField]
        private GameSettingsUI _settingsPanelUI;

        // NOVO: Refer�ncia ao CanvasGroup do indicador de carregamento
        [Header("Indicador de Status")]
        [SerializeField]
        private CanvasGroup _loadingSpinnerCanvasGroup;


        // --- Ciclo de Vida e Setup ---

        private void Start()
        {
            // ... (A��es de rede e UI) ...
            _buttonHost.onClick.AddListener(OnButtonStartHost);
            _buttonJoin.onClick.AddListener(OnButtonJoinServer);
            _buttonLobbies.onClick.AddListener(OnButtonShowLobbies);
            _buttonSettings.onClick.AddListener(OnButtonShowSettings);
            _buttonQuit.onClick.AddListener(OnButtonQuit);

            if (_settingsPanelUI != null)
            {
                _settingsPanelUI.OnPanelClosed += OnSettingsPanelClosed;
                _settingsPanelUI.gameObject.SetActive(false);
            }

            // Garante que o indicador de carregamento comece escondido
            if (_loadingSpinnerCanvasGroup != null)
            {
                _loadingSpinnerCanvasGroup.alpha = 0;
                _loadingSpinnerCanvasGroup.blocksRaycasts = false;
            }
        }

        private void OnDestroy()
        {
            if (_settingsPanelUI != null)
            {
                _settingsPanelUI.OnPanelClosed -= OnSettingsPanelClosed;
            }
        }

        // --- Controle de Intera��o dos Bot�es e Loading ---

        private void SetButtonsInteractable(bool interactable)
        {
            // 1. Define a interatividade dos bot�es de menu
            _buttonHost.interactable = interactable;
            _buttonJoin.interactable = interactable;
            _buttonLobbies.interactable = interactable;
            _buttonSettings.interactable = interactable;
            _buttonQuit.interactable = interactable;
            _inputFieldJoinCode.interactable = interactable;

            // 2. Controla o indicador de loading
            if (_loadingSpinnerCanvasGroup != null)
            {
                // Se N�O est� interativo, mostra o spinner (loading)
                if (!interactable)
                {
                    ShowLoadingSpinner();
                }
                else
                {
                    HideLoadingSpinner();
                }
            }
        }

        private void ShowLoadingSpinner()
        {
            // Torna vis�vel e bloqueia raycasts para evitar cliques
            _loadingSpinnerCanvasGroup.alpha = 1;
            _loadingSpinnerCanvasGroup.blocksRaycasts = true;
        }

        private void HideLoadingSpinner()
        {
            // Torna invis�vel e permite intera��o com o menu
            _loadingSpinnerCanvasGroup.alpha = 0;
            _loadingSpinnerCanvasGroup.blocksRaycasts = false;
        }

        // --- Fun��es de Rede (Uso de try/finally mantido) ---

        public async void OnButtonStartHost()
        {
            SetButtonsInteractable(false);
            try
            {
                await NetHandlerHost.Instance.StartHostAsync();
            }
            finally
            {
                SetButtonsInteractable(true);
            }
        }

        public async void OnButtonJoinServer()
        {
            SetButtonsInteractable(false);
            try
            {
                await NetHandlerClient.Instance.JoinGameAsync(_inputFieldJoinCode.text);
            }
            finally
            {
                SetButtonsInteractable(true);
            }
        }

        // --- Fun��es de Navega��o e UI ---

        public void OnButtonShowLobbies()
        {
            // Assumindo que UnCanvas.ToggleCanvasGroup gerencia o alpha/interactable de forma suave
            UnCanvas.ToggleCanvasGroup(_canvasGroupLobbies, true, 0.5f);
        }

        public void OnButtonShowSettings()
        {
            if (_settingsPanelUI != null)
            {
                _settingsPanelUI.gameObject.SetActive(true);
            }
        }

        private void OnSettingsPanelClosed()
        {
            if (_buttonSettings != null)
            {
                EventSystem.current.SetSelectedGameObject(_buttonSettings.gameObject);
            }
        }

        public void OnButtonQuit()
        {
            Debug.Log("Saindo do Jogo...");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }
    }
}