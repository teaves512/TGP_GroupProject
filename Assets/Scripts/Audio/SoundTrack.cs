using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundTrack
{
    [SerializeField] public string m_Name;
    [SerializeField] public AudioClip m_Clip;
    [SerializeField] public float m_Volume = 1.0f;
}
