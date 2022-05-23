using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

public class UIManager : MonoBehaviour
{
    #region Variables
    public static UIManager Instance;

    [Header("UI Panels")]
    [SerializeField] private RectTransform m_PanelSetup;
    [SerializeField] private RectTransform m_PanelGameBoard;
    [SerializeField] private RectTransform m_PanelGameOver;

    [Header("Setup Panel")]
    [SerializeField] private TMP_InputField m_InputFieldName;
    private PlayerIcon m_SelectedIcon = PlayerIcon.RED;

    [SerializeField] private Button m_ButtonAddPlayer;
    [SerializeField] private Button m_ButtonStartGame;

    [SerializeField] private Button m_ButtonIconRed;
    [SerializeField] private Button m_ButtonIconGreen;
    [SerializeField] private Button m_ButtonIconBlue;
    [SerializeField] private Button m_ButtonIconYellow;
    [SerializeField] private List<Outline> m_OutlinesButtonIcons;

    [SerializeField] private RectTransform m_PanelAddedPlayers;
    [SerializeField] private GameObject m_PanelPlayerSetup;

    [SerializeField] private TMP_Text m_InputWarningText;
    [SerializeField] private float m_TextFadeRate;
    [SerializeField] private float m_TextFadeLength;

    [Header("Leaderboard")]
    [SerializeField] private RectTransform m_PanelLeaderboard;
    [SerializeField] private GameObject m_PanelPlayerInfo;
    private List<RectTransform> m_PlayerInfoPanels = new List<RectTransform>();

    [Header("Pause")]
    [SerializeField] private RectTransform m_PanelPause;
    [SerializeField] private RectTransform m_PanelBackToMenu;
    [SerializeField] private RectTransform m_PanelSettings;
    [SerializeField] private Button m_ButtonPause;

    [Header("Settings")]
    [SerializeField] private AudioMixer m_Mixer;
    [SerializeField] private Slider m_SliderMasterVolume;
    [SerializeField] private Slider m_SliderSFXVolume;
    [SerializeField] private Slider m_SliderMusicVolume;

    [SerializeField] private TMP_Dropdown m_DropdownResolution;
    private int[] m_Resolution = { 0, 0 };
    [SerializeField] private TMP_Dropdown m_DropdownQuality;
    [SerializeField] private Toggle m_ToggleFullscreen;
    [SerializeField] private Toggle m_TogglePostProcessing;

    [Header("URP Quality")]
    [SerializeField] private List<UniversalRenderPipelineAsset> m_URPQualityLevels = new List<UniversalRenderPipelineAsset>();
    #endregion

    private void Awake()
    {
        Instance = this;

        GameManager.OnSetGameState += GameManager_OnSetGameState;

        InitializeSettings();
    }

    private void OnDestroy()
    {
        GameManager.OnSetGameState -= GameManager_OnSetGameState;
    }

    private void GameManager_OnSetGameState(GameState _state)
    {
        m_PanelSetup        .gameObject.SetActive(_state == GameState.SETUP);
        m_PanelGameBoard    .gameObject.SetActive(_state == GameState.GAME_BOARD);
        m_PanelGameOver     .gameObject.SetActive(_state == GameState.GAME_OVER);
        m_PanelLeaderboard  .gameObject.SetActive(_state == GameState.GAME_BOARD);
        m_PanelPause        .gameObject.SetActive(_state == GameState.PAUSE);

        m_ButtonPause.gameObject.SetActive(
            _state == GameState.GAME_BOARD ||
            _state == GameState.MINIGAME_BOMB_TAG ||
            _state == GameState.MINIGAME_DEATHMATCH ||
            _state == GameState.MINIGAME_KOTH ||
            _state == GameState.MINIGAME_RACE 
            || _state == GameState.MINIGAME_BALLOONPOP);
        if (_state != GameState.PAUSE)
        {
            m_PanelPause        .gameObject.SetActive(false);
            m_PanelBackToMenu   .gameObject.SetActive(false);
            m_PanelSettings     .gameObject.SetActive(false);
        }
    }

    #region Setup
    public void ResetSetupMenu()
    {
        m_InputFieldName.text = "";
        m_ButtonAddPlayer.interactable = false;
        m_ButtonStartGame.interactable = false;

        m_PanelLeaderboard.gameObject.SetActive(false);
        foreach (RectTransform panel in m_PlayerInfoPanels)
        {
            Destroy(panel.gameObject);
            m_PlayerInfoPanels.Remove(panel);
        }
        for (int i = 0; i < m_PanelAddedPlayers.childCount; i++)
        {
            Destroy(m_PanelAddedPlayers.GetChild(i).gameObject);
        }
    }

    public void NameInputChanged()
    {
        if (GameManager.s_Instance.GetPlayers().Count < 4) { m_ButtonAddPlayer.interactable = m_InputFieldName.text != ""; }
        else { m_ButtonAddPlayer.interactable = false; }
    }

    public void SelectIcon(int _index)
    {
        switch(_index)
        {
            case 0:
                m_SelectedIcon = PlayerIcon.RED;
                OutlineIconButton(0);
                break;
            case 1:
                m_SelectedIcon = PlayerIcon.GREEN;
                OutlineIconButton(1);
                break;
            case 2:
                m_SelectedIcon = PlayerIcon.BLUE;
                OutlineIconButton(2);
                break;
            case 3:
                m_SelectedIcon = PlayerIcon.YELLOW;
                OutlineIconButton(3);
                break;
            default:
                m_SelectedIcon = PlayerIcon.RED;
                OutlineIconButton(0);
                Debug.LogWarning("Icon button index out of range. Icon defaulted to red.");
                break;
        }
    }

    private void OutlineIconButton(int _indexToOutline)
    {
        for (int i = 0; i < 4; i++) { m_OutlinesButtonIcons[i].enabled = (i == _indexToOutline); }
    }

    public void AddPlayer()
    {
        GameManager.s_Instance.AddPlayer(m_InputFieldName.text, m_SelectedIcon);
        m_InputFieldName.text = "";

        //set the icon button that has just been used to non-interactable
        switch (m_SelectedIcon)
        {
            case PlayerIcon.RED:
                m_ButtonIconRed.interactable = false;
                break;
            case PlayerIcon.GREEN:
                m_ButtonIconGreen.interactable = false;
                break;
            case PlayerIcon.BLUE:
                m_ButtonIconBlue.interactable = false;
                break;
            case PlayerIcon.YELLOW:
                m_ButtonIconYellow.interactable = false;
                break;
        }

        UpdateSetupMenu();
    }

    public void RemovePlayer()
    {
        //remove player
        //...

        UpdateSetupMenu();
    }

    public void UpdateSetupMenu()
    {
        foreach (RectTransform child in m_PanelAddedPlayers)
        { Destroy(child.gameObject); }

        foreach (PlayerInfo player in GameManager.s_Instance.GetPlayers())
        {
            RectTransform panelPlayerSetup = Instantiate(m_PanelPlayerSetup, m_PanelAddedPlayers.position, Quaternion.identity, m_PanelAddedPlayers).GetComponent<RectTransform>();

            Color iconColor = new Color();
            switch (player.GetIcon())
            {
                case PlayerIcon.RED:
                    iconColor = Color.red;
                    break;
                case PlayerIcon.GREEN:
                    iconColor = Color.green;
                    break;
                case PlayerIcon.BLUE:
                    iconColor = Color.blue;
                    break;
                case PlayerIcon.YELLOW:
                    iconColor = Color.yellow;
                    break;
            }

            panelPlayerSetup.GetChild(0).GetComponent<Image>().color = iconColor;
            panelPlayerSetup.GetChild(1).GetComponent<TMP_Text>().text = player.GetName();
        }

        m_PanelAddedPlayers.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GameManager.s_Instance.GetPlayers().Count * 70.0f);

        Vector2 addedPlayersPos = m_PanelAddedPlayers.anchoredPosition;
        addedPlayersPos.y = (m_PanelAddedPlayers.sizeDelta.y / -2) - 200.0f;
        m_PanelAddedPlayers.anchoredPosition = addedPlayersPos;

        //if there is 2-4 players, allow the game to start
        m_ButtonStartGame.interactable = GameManager.s_Instance.GetPlayers().Count > 1;

        //set the new defualt value for the selected icon
        if (m_ButtonIconRed.interactable)           { SelectIcon(0); }
        else if (m_ButtonIconGreen.interactable)    { SelectIcon(1); }
        else if (m_ButtonIconBlue.interactable)     { SelectIcon(2); }
        else if (m_ButtonIconYellow.interactable)   { SelectIcon(3); }
    }

    public void FinishSetup()
    {
        if (GameManager.s_Instance.AreAllPlayerControlsAssigned())
            StartGame();
        else
            StartCoroutine(WarningTextCoroutine());
    }

    IEnumerator WarningTextCoroutine()
    {
        float initialTime = Time.time;
        float currentTime = Time.time;
        while ((currentTime - initialTime) < m_TextFadeLength)
        {
            m_InputWarningText.alpha = Mathf.Abs(Mathf.Sin((currentTime - initialTime) * m_TextFadeRate * Mathf.PI));

            yield return new WaitForFixedUpdate();
            currentTime = Time.time;
        }

        m_InputWarningText.alpha = 0;
    }

    #endregion

    #region Game
    public void UpdateLeaderboardPanel()
    {
        m_PanelLeaderboard.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GameManager.s_Instance.GetPlayers().Count * 80.0f);

        Vector2 leaderboardPos = m_PanelLeaderboard.anchoredPosition;
        leaderboardPos.y = m_PanelLeaderboard.sizeDelta.y / -2;
        m_PanelLeaderboard.anchoredPosition = leaderboardPos;

        for (int i = 0; i < GameManager.s_Instance.GetPlayers().Count; i++)
        {
            string playerInfoText = "";
            switch (GameManager.s_Instance.GetPlayers()[i].GetIcon())
            {
                case PlayerIcon.RED:
                    playerInfoText += "R";
                    break;
                case PlayerIcon.GREEN:
                    playerInfoText += "G";
                    break;
                case PlayerIcon.BLUE:
                    playerInfoText += "B";
                    break;
                case PlayerIcon.YELLOW:
                    playerInfoText += "Y";
                    break;
            }
            playerInfoText +=
                " | " + GameManager.s_Instance.GetPlayers()[i].GetName() +
                " | " + GameManager.s_Instance.GetPlayers()[i].GetScore().ToString();
            m_PlayerInfoPanels[i].GetChild(0).GetComponent<TMP_Text>().text = playerInfoText;
        }
    }

    public void StartGame()
    {
        foreach (PlayerInfo player in GameManager.s_Instance.GetPlayers())
        {
            RectTransform playerInfoPanel = Instantiate(m_PanelPlayerInfo, m_PanelLeaderboard.position, Quaternion.identity, m_PanelLeaderboard).GetComponent<RectTransform>();
            m_PlayerInfoPanels.Add(playerInfoPanel);
        }
        UpdateLeaderboardPanel();

        GameManager.s_Instance.SetGameState(GameState.GAME_BOARD, true);
    }
    #endregion

    #region PauseMenu
    public void Pause()
    {
        GameManager.s_Instance.SetGameState(GameState.PAUSE);
    }

    public void Resume()
    {
        GameManager.s_Instance.SetGameState(GameManager.s_Instance.GetLastGameState());
    }

    public void Settings()
    {
        m_PanelSettings.gameObject.SetActive(true);
        m_PanelPause.gameObject.SetActive(false);
    }

    public void SettingsToPause()
    {
        m_PanelPause.gameObject.SetActive(true);
        m_PanelSettings.gameObject.SetActive(false);
    }

    public void BackToMenu()
    {
        m_PanelBackToMenu.gameObject.SetActive(true);
        m_PanelPause.gameObject.SetActive(false);
    }

    public void BackToMenu_No()
    {
        m_PanelPause.gameObject.SetActive(true);
        m_PanelBackToMenu.gameObject.SetActive(false);
    }

    public void BackToMenu_Yes()
    {
        m_PanelBackToMenu.gameObject.SetActive(false);
        GameManager.s_Instance.SetGameState(GameState.MAIN_MENU);
    }
    #endregion

    #region Settings
    private void InitializeSettings()
    {
        m_DropdownResolution.value = PlayerPrefs.GetInt("RESOLUTION", 1);
        m_DropdownQuality.value = PlayerPrefs.GetInt("QUALITY_LEVEL", 2);
        m_ToggleFullscreen.isOn = PlayerPrefs.GetInt("FULLSCREEN", 1) > 0;
        m_TogglePostProcessing.isOn = PlayerPrefs.GetInt("POST_PROCESSING", 1) > 0;

        m_SliderMasterVolume.value = PlayerPrefs.GetFloat("MASTER_VOLUME", 10);
        m_SliderSFXVolume.value = PlayerPrefs.GetFloat("SFX_VOLUME", 10);
        m_SliderMusicVolume.value = PlayerPrefs.GetFloat("MUSIC_VOLUME", 10);

        OnResolutionChanged();
        OnQualityChanged();
        OnFullscreenToggled();
        OnPostProcessingToggled();

        MasterVolumeChanged();
        SFXVolumeChanged();
        MusicVolumeChanged();
    }

    public void MasterVolumeChanged()
    {
        float volume = m_SliderMasterVolume.value;
        PlayerPrefs.SetFloat("MASTER_VOLUME", volume);
        volume = -80 * (1.0f - (volume / 10.0f));

        m_Mixer.SetFloat("MasterVolume", volume);
    }

    public void SFXVolumeChanged()
    {
        float volume = m_SliderSFXVolume.value;
        PlayerPrefs.SetFloat("SFX_VOLUME", volume);
        volume = -80 * (1.0f - (volume / 10.0f));

        m_Mixer.SetFloat("SFXVolume", volume);
    }

    public void MusicVolumeChanged()
    {
        float volume = m_SliderMusicVolume.value;
        PlayerPrefs.SetFloat("MUSIC_VOLUME", volume);
        volume = -80 * (1.0f - (volume / 10.0f));

        m_Mixer.SetFloat("MusicVolume", volume);
    }

    public void OnResolutionChanged()
    {
        int resolutionIndex = m_DropdownResolution.value;
        PlayerPrefs.SetInt("RESOLUTION", resolutionIndex);
        switch (resolutionIndex)
        {
            case 0:
                m_Resolution[0] = 1280;
                m_Resolution[1] = 720;
                break;

            case 1:
                m_Resolution[0] = 1920;
                m_Resolution[1] = 1080;
                break;

            case 2:
                m_Resolution[0] = 2560;
                m_Resolution[1] = 1440;
                break;

            case 3:
                m_Resolution[0] = 3840;
                m_Resolution[1] = 2160;
                break;

            default:
                m_Resolution[0] = 1920;
                m_Resolution[1] = 1080;
                break;
        }
        SetScreenSize();
    }

    public void OnFullscreenToggled()
    {
        bool isFullscreen = m_ToggleFullscreen.isOn;
        PlayerPrefs.SetInt("FULLSCREEN", isFullscreen ? 1 : 0);
        SetScreenSize();
    }

    private void SetScreenSize() { Screen.SetResolution(m_Resolution[0], m_Resolution[1], m_ToggleFullscreen.isOn); }

    public void OnQualityChanged()
    {
        int qualityIndex = m_DropdownQuality.value;
        PlayerPrefs.SetInt("QUALITY_LEVEL", qualityIndex);

        QualitySettings.SetQualityLevel(qualityIndex);
        QualitySettings.renderPipeline = m_URPQualityLevels[qualityIndex];
    }
    public void OnPostProcessingToggled()
    {
        bool isOn = m_TogglePostProcessing.isOn;
        PlayerPrefs.SetInt("POST_PROCESSING", isOn ? 1 : 0);
    }
    #endregion

    /* OnGUI() {...}
    private void OnGUI()
    {
        if (GameManager.s_Instance.GetGameState() == GameState.GAME_BOARD)
        {
            for (int i = 0; i < GameManager.s_Instance.GetPlayers().Count; i++)
            {
                if (GUI.Button(new Rect(10 + (i * 60), 60, 50, 50), GameManager.s_Instance.GetPlayers()[i].GetName()))
                {
                    GameManager.s_Instance.GetPlayers()[i].AwardPoints(100);
                }
            }
        }
    }
    */
}
