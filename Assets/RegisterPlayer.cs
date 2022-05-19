using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterPlayer : ButtonTemplate
{

    public TMP_InputField m_PlayerName;
    
    protected override void ButtonAction()
    {
        UserManager.Instance.Load();
        if (UserManager.Instance.m_User.HasRegistered)
        {
            m_NextSceneName = "Scene2";
            ChangeScene();
            return;
        }
        if (m_PlayerName.text != null)
        {
            UserManager.Instance.UpdateUser(m_PlayerName.text);
            ChangeScene();
        }
        
        UserManager.Instance.m_User.PlayersAchievements.AddBombsDropped();

    }
    
}
