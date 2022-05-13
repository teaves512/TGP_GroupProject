using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class ButtonTemplate : MonoBehaviour
{
    [SerializeField]public string m_NextSceneName, m_CurrentSceneName;
    protected abstract void ButtonAction();
    [SerializeField] private Button m_ButtonAction;
    protected virtual void Awake()
    {
        m_ButtonAction = GetComponent<Button>();
            
        m_ButtonAction.onClick.AddListener(ButtonAction);
    }

    protected void ChangeScene()
    {
        ScenesManager.Instance.ChangeScene(m_NextSceneName, m_CurrentSceneName );
    }
    
}
