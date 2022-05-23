using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

public class UIManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AudioMixer m_Mixer;
    [SerializeField] private Slider m_SliderMasterVolume;
    [SerializeField] private Slider m_SliderAmbienceVolume;
    [SerializeField] private Slider m_SliderMusicVolume;
    [SerializeField] private Slider m_SliderSFXVolume;

    [SerializeField] private TMP_Dropdown m_DropdownResolution;
    private int[] m_Resolution = { 0, 0 };
    [SerializeField] private TMP_Dropdown m_DropdownQuality;
    [SerializeField] private Toggle m_ToggleFullscreen;
    [SerializeField] private Toggle m_TogglePostProcessing;

    [Header("URP Quality")]
    [SerializeField] private List<UniversalRenderPipelineAsset> m_URPQualityLevels = new List<UniversalRenderPipelineAsset>();

    private void Start()
    {
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        m_DropdownResolution.value = PlayerPrefs.GetInt("RESOLUTION", 1);
        m_DropdownQuality.value = PlayerPrefs.GetInt("QUALITY_LEVEL", 2);
        m_ToggleFullscreen.isOn = PlayerPrefs.GetInt("FULLSCREEN", 1) > 0;
        m_TogglePostProcessing.isOn = PlayerPrefs.GetInt("POST_PROCESSING", 1) > 0;

        m_SliderMasterVolume.value = PlayerPrefs.GetFloat("MASTER_VOLUME", 10);
        m_SliderSFXVolume.value = PlayerPrefs.GetFloat("AMBIENCE_VOLUME", 10);
        m_SliderMusicVolume.value = PlayerPrefs.GetFloat("MUSIC_VOLUME", 10);
        m_SliderSFXVolume.value = PlayerPrefs.GetFloat("SFX_VOLUME", 10);

        OnResolutionChanged();
        OnQualityChanged();
        OnFullscreenToggled();
        OnPostProcessingToggled();

        MasterVolumeChanged();
        AmbienceVolumeChanged();
        MusicVolumeChanged();
        SFXVolumeChanged();
    }

    public void MasterVolumeChanged()
    {
        float volume = m_SliderMasterVolume.value;
        PlayerPrefs.SetFloat("MASTER_VOLUME", volume);
        volume = -80 * (1.0f - (volume / 10.0f));

        m_Mixer.SetFloat("MasterVolume", volume);
    }

    public void AmbienceVolumeChanged()
    {
        float volume = m_SliderAmbienceVolume.value;
        PlayerPrefs.SetFloat("AMBIENCE_VOLUME", volume);
        volume = -80 * (1.0f - (volume / 10.0f));

        m_Mixer.SetFloat("AmbienceVolume", volume);
    }

    public void MusicVolumeChanged()
    {
        float volume = m_SliderMusicVolume.value;
        PlayerPrefs.SetFloat("MUSIC_VOLUME", volume);
        volume = -80 * (1.0f - (volume / 10.0f));

        m_Mixer.SetFloat("MusicVolume", volume);
    }

    public void SFXVolumeChanged()
    {
        float volume = m_SliderSFXVolume.value;
        PlayerPrefs.SetFloat("SFX_VOLUME", volume);
        volume = -80 * (1.0f - (volume / 10.0f));

        m_Mixer.SetFloat("SFXVolume", volume);
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
                m_Resolution[0] = 2160;
                m_Resolution[1] = 1440;
                break;

            case 3:
                m_Resolution[0] = 2560;
                m_Resolution[1] = 1440;
                break;

            case 4:
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
}
