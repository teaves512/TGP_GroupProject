using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    [SerializeField] public string m_Name;

    [SerializeField] public AudioClip m_Clip;

    [HideInInspector] public AudioSource m_Source;
    [SerializeField] public AudioMixerGroup m_MixerGroup;

    [SerializeField] public float m_Volume = 1.0f;
    [SerializeField] public float m_Pitch = 1.0f;

    [SerializeField] public bool m_bLoop = false;
    [SerializeField] public bool m_bPlayOnAwake = false;
}
