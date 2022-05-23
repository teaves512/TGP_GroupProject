using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Volume))]
public class CameraPostProcessing : MonoBehaviour
{
    private Volume m_Volume;

    private void Start()
    {
        m_Volume = GetComponent<Volume>();
        EnablePostProcessing();
    }

    public void EnablePostProcessing()
    {
        m_Volume.enabled = PlayerPrefs.GetInt("POST_PROCESSING", 1) > 0;
    }
}
