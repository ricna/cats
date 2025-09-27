using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Unrez.BackyardShowdown
{
    public class GameSettingsUI : MonoBehaviour
    {

        private Controls _controls;

        [Header("UI Components")]
        [SerializeField]
        private Slider _musicVolumeSlider;
        [SerializeField]
        private Slider _sfxVolumeSlider;

        [SerializeField]
        private Toggle _toggleVsync;
        [SerializeField]
        private Toggle _toggleFullscreen;

        [SerializeField]
        private ButtonHandler _btnLanguage; // Assumindo 'ButtonHandler' � um script customizado
        [SerializeField]
        private ButtonHandler _btnBack; // Bot�o de Voltar/Aplicar

        [Header("Language Settings")]
        [SerializeField]
        private SystemLanguage[] _availableLanguages;
        private int _currentLanguageIndex = 0;


        public Action OnPanelClosed;

        // --- Ciclo de Vida do MonoBehaviour ---

        private void Awake()
        {
            // 1. Configurar o Input System (se 'Controls' for o novo Input System)
            _controls = new Controls();
            // Os listeners do Input ser�o habilitados em OnEnable

            // 2. Configurar Listeners da UI
            if (_btnLanguage != null)
            {
                _btnLanguage.onClick += OnChangeLanguageClick;
            }

            // Os Handlers de Sliders e Toggles agora apenas atualizam o objeto em mem�ria e aplicam Imediatamente
            _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            _sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            _toggleVsync.onValueChanged.AddListener(OnToggleVsyncChanged);
            _toggleFullscreen.onValueChanged.AddListener(OnToggleFullscreenChanged);

            // Listener para o bot�o de voltar, garantindo que ele tamb�m salve
            if (_btnBack != null)
            {
                _btnBack.onClick += Back;
            }
        }

        private void OnEnable()
        {
            // Garante que o input e a UI est�o ativos e carregados
            _controls.UI.Enable();
            _controls.UI.Back.performed += ctx => Back();
            _controls.UI.Move.performed += OnMovePerformed;

            // Carrega os dados salvos para a UI
            LoadUISettings();

            // Foca no primeiro elemento
            if (_musicVolumeSlider != null)
            {
                EventSystem.current.SetSelectedGameObject(_musicVolumeSlider.gameObject);
            }
        }

        private void OnDisable()
        {
            // Remove handlers do Input System
            _controls.UI.Move.performed -= OnMovePerformed;
            _controls.UI.Back.performed -= ctx => Back();
            _controls.UI.Disable();

            // Salva as altera��es feitas no disco quando o painel � desabilitado
            SaveSettingsToDisk();
        }

        // --- Handlers de Input ---

        public void OnMovePerformed(InputAction.CallbackContext ctx)
        {
            Vector2 moveDirection = ctx.ReadValue<Vector2>();
            // Permite trocar a linguagem usando o input horizontal no gamepad/teclado
            if (moveDirection.x != 0 && EventSystem.current.currentSelectedGameObject == _btnLanguage?.gameObject)
            {
                OnChangeLanguage(moveDirection.x > 0);
            }
        }

        // --- M�todos de Volume ---

        public void OnMusicVolumeChanged(float value)
        {
            GameSettingsManager.Settings.musicVolume = value; // 1. Atualiza a inst�ncia em mem�ria
            AudioManager.Instance.SetMusicVolume(value);       // 2. Aplica imediatamente
        }

        public void OnSFXVolumeChanged(float value)
        {
            GameSettingsManager.Settings.sfxVolume = value;   // 1. Atualiza a inst�ncia em mem�ria
            AudioManager.Instance.SetSFXVolume(value);         // 2. Aplica imediatamente
        }

        // --- M�todos de Linguagem ---

        private void OnChangeLanguageClick()
        {
            OnChangeLanguage(true); // Chamada pelo clique do bot�o (avan�a)
        }

        private void OnChangeLanguage(bool forward)
        {
            // L�gica de loop para o �ndice de linguagem
            _currentLanguageIndex += forward ? 1 : -1;
            if (_currentLanguageIndex >= _availableLanguages.Length)
            {
                _currentLanguageIndex = 0;
            }
            else if (_currentLanguageIndex < 0)
            {
                _currentLanguageIndex = _availableLanguages.Length - 1;
            }

            ApplyNewLanguage();
        }

        private void ApplyNewLanguage()
        {
            SystemLanguage newLanguage = _availableLanguages[_currentLanguageIndex];

            // 1. Atualiza a inst�ncia em mem�ria
            GameSettingsManager.Settings.language = newLanguage;

            // 2. Atualiza a UI e chama o sistema de localiza��o (se existir)
            UpdateLanguageButtonText(newLanguage);
            // Exemplo: LocalizationManager.Instance.SetLanguage(newLanguage);
        }

        private void UpdateLanguageButtonText(SystemLanguage lang)
        {
            if (_btnLanguage != null)
            {
                // Note: O nome do Enum � usado como texto, idealmente seria uma string localizada.
                _btnLanguage.GetComponentInChildren<TMP_Text>().text = lang.ToString();
            }
        }

        // --- M�todos de Toggle ---

        private void OnToggleVsyncChanged(bool isOn)
        {
            GameSettingsManager.Settings.vsync = isOn;
            QualitySettings.vSyncCount = isOn ? 1 : 0; // Aplica imediatamente
        }

        private void OnToggleFullscreenChanged(bool isOn)
        {
            // Aten��o: A propriedade Screen.fullScreen s� deve ser alterada se for diferente do valor atual 
            // para evitar opera��es desnecess�rias do sistema operacional.
            if (Screen.fullScreen != isOn)
            {
                GameSettingsManager.Settings.fullscreen = isOn;
                Screen.fullScreen = isOn; // Aplica imediatamente
            }
        }

        // --- Salvamento, Carregamento e Navega��o ---

        private void Back()
        {
            OnPanelClosed?.Invoke();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Salva todas as configura��es atuais do objeto est�tico para o disco. 
        /// Chamado apenas no OnDisable.
        /// </summary>
        public void SaveSettingsToDisk()
        {
            GameSettingsManager.SaveSettings();
        }

        /// <summary>
        /// Carrega as configura��es do GameSettingsManager e popula a UI.
        /// </summary>
        public void LoadUISettings()
        {
            // O acesso a GameSettingsManager.Settings garante que os dados estejam carregados do disco.
            GameSettingsData currentSettings = GameSettingsManager.Settings;

            // 1. Aplicar os valores salvos aos Sliders/Toggles
            _musicVolumeSlider.value = currentSettings.musicVolume;
            _sfxVolumeSlider.value = currentSettings.sfxVolume;
            _toggleVsync.isOn = currentSettings.vsync;

            // Usamos o Screen.fullScreen como fonte da verdade para a UI ao carregar
            _toggleFullscreen.isOn = Screen.fullScreen;

            // 2. Localizar o �ndice da linguagem salva
            _currentLanguageIndex = Array.IndexOf(_availableLanguages, currentSettings.language);
            if (_currentLanguageIndex < 0)
            {
                // Se a linguagem salva n�o estiver na lista (erro), volta para a primeira
                _currentLanguageIndex = 0;
            }

            // 3. Atualizar o texto da UI
            UpdateLanguageButtonText(_availableLanguages[_currentLanguageIndex]);

            // 4. Garante que as configura��es estejam aplicadas aos sistemas do jogo (Audio, Quality, Screen)
            // Isso � crucial caso o menu seja aberto no meio do jogo.
            GameSettingsManager.ApplySettings();
        }
    }
}