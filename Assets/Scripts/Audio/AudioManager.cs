using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager s_Instance;

    [SerializeField] private List<Sound> m_Sounds = new List<Sound>();
    [SerializeField] private List<SoundTrack> m_MusicTracks = new List<SoundTrack>();
    [SerializeField] private List<SoundTrack> m_AmbienceTracks = new List<SoundTrack>();

    private AudioSource m_Music;
    private AudioSource m_Ambience;

    [SerializeField] private AudioMixerGroup m_MusicMixerGroup;
    [SerializeField] private AudioMixerGroup m_AmbienceMixerGroup;

    private void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        s_Instance = this;
        DontDestroyOnLoad(this);

        foreach (Sound sound in m_Sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();

            source.clip = sound.m_Clip;
            source.outputAudioMixerGroup = sound.m_MixerGroup;
            source.volume = sound.m_Volume;
            source.pitch = sound.m_Pitch;
            source.loop = sound.m_bLoop;
            source.playOnAwake = sound.m_bPlayOnAwake;

            sound.m_Source = source;

            if (sound.m_bPlayOnAwake) { source.Play(); }
        }

        m_Music = gameObject.AddComponent<AudioSource>();
        m_Music.loop = true;
        m_Music.playOnAwake = false;
        m_Music.outputAudioMixerGroup = m_MusicMixerGroup;

        m_Ambience = gameObject.AddComponent<AudioSource>();
        m_Ambience.loop = true;
        m_Ambience.playOnAwake = false;
        m_Ambience.outputAudioMixerGroup = m_AmbienceMixerGroup;
    }

    public static void Play(string _name)
    {
        if (s_Instance != null) { s_Instance.PlaySound(_name); }
        else { Debug.LogWarning("No instance of the AudioManager class exists."); }
    }

    public void PlaySound(string _name)
    {
        Sound soundToPlay = null;
        foreach (Sound sound in m_Sounds)
        {
            if (sound.m_Name == _name)
            {
                soundToPlay = sound;
                break;
            }
        }
        if (soundToPlay == null)
        {
            Debug.LogWarning("Cannot find sound with name: " + _name);
            return;
        }
        soundToPlay.m_Source.PlayOneShot(soundToPlay.m_Clip);
    }

    public static void SetMusic(string _name)
    {
        if (s_Instance != null) { s_Instance.Instance_SetMusic(_name); }
        else { Debug.LogWarning("No instance of the AudioManager class exists."); }
    }

    private void Instance_SetMusic(string _name)
    {
        SoundTrack trackToPlay = null;
        foreach (SoundTrack track in m_MusicTracks)
        {
            if (track.m_Name == _name)
            {
                trackToPlay = track;
                break;
            }
        }
        if (trackToPlay == null)
        {
            Debug.LogWarning("Cannot find music track with name: " + _name);
            return;
        }
        m_Music.clip = trackToPlay.m_Clip;
        m_Music.volume = trackToPlay.m_Volume;
    }

    public static void PlayMusic(bool _bPlay = true)
    {
        if (s_Instance != null) { s_Instance.Instance_PlayMusic(_bPlay); }
        else { Debug.LogWarning("No instance of the AudioManager class exists."); }
    }

    private void Instance_PlayMusic(bool _bPlay = true)
    {
        if (_bPlay) { m_Music.Play(); }
        else { m_Music.Stop(); }
    }

    public static void SetAmbience(string _name)
    {
        if (s_Instance != null) { s_Instance.Instance_SetAmbience(_name); }
        else { Debug.LogWarning("No instance of the AudioManager class exists."); }
    }

    private void Instance_SetAmbience(string _name)
    {
        SoundTrack trackToPlay = null;
        foreach (SoundTrack track in m_AmbienceTracks)
        {
            if (track.m_Name == _name)
            {
                trackToPlay = track;
                break;
            }
        }
        if (trackToPlay == null)
        {
            Debug.LogWarning("Cannot find ambience track with name: " + _name);
            return;
        }
        m_Ambience.clip = trackToPlay.m_Clip;
        m_Ambience.volume = trackToPlay.m_Volume;
    }

    public static void PlayAmbience(bool _bPlay = true)
    {
        if (s_Instance != null) { s_Instance.Instance_PlayMusic(_bPlay); }
        else { Debug.LogWarning("No instance of the AudioManager class exists."); }
    }

    private void Instance_PlayAmbience(bool _bPlay = true)
    {
        if (_bPlay) { m_Ambience.Play(); }
        else { m_Ambience.Stop(); }
    }
}