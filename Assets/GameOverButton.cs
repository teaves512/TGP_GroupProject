using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverButton : ChangeSceneButton
{
    public enum buttontype
    {
        CONTINUE,
        RETRY,
        EXIT
    }

    public GameObject m_ContinueButton;
    protected override void Awake()
    {
        if (m_ContinueButton.activeInHierarchy)
        {
            m_ContinueButton.gameObject.SetActive(false);
        }

        EventManager.GameOver += GameOver;
        CheckPersistentScene();
        base.Awake();
    }


    [SerializeField] private buttontype M_GameOverScreenButton;
    protected override void ButtonAction()
    {
        switch (M_GameOverScreenButton)
        {
            case buttontype.CONTINUE:
                ContinueButtonFunction();
                break;
            case buttontype.RETRY:
                RetryButtonFunction();
                break;
            case buttontype.EXIT:
                ExitButtonFunction();
                break;
            default:
                break;
        }
        base.ButtonAction();
        
    }

    void GameOver(bool victory)
    {
        if (victory && !m_ContinueButton.activeInHierarchy)
        {
            m_ContinueButton.gameObject.SetActive(true);
        }
    }

    void ContinueButtonFunction()
    {
        
    }
    
    void RetryButtonFunction()
    {
        
    }
    
    void ExitButtonFunction()
    {
        
    }
    
}
