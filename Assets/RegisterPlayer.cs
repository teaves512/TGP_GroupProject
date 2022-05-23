using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterPlayer : ChangeSceneButton
{
    public TMP_InputField m_PlayerName;

    protected override void ButtonAction()
    {
        UserManager userManager = FindObjectOfType<UserManager>();

        userManager.Load();
        if (userManager.m_User.HasRegistered)
        {
            LoadScene("Scene2");
            return;
        }
        if (m_PlayerName.text != null)
        {
            userManager.UpdateUser(m_PlayerName.text);
            LoadScene("Scene2");
        }
    }
    
}
