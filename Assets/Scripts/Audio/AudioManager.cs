using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager s_Instance;

    [SerializeField] private List<Sound> m_Sounds = new List<Sound>();

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
    }

    public static void Play(string _name) { s_Instance.PlaySound(_name); }

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
            Debug.LogWarning("No sound with name: " + _name);
            return;
        }
        soundToPlay.m_Source.Play();
    }
}