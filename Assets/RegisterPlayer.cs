using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterPlayer : ButtonTemplate
{

    public TMP_InputField m_PlayerName;
    
    protected override void ButtonAction()
    {
        if (m_PlayerName.text == null)
        {
            UserManager.Instance.UpdateUser(m_PlayerName.text);
        }
        ChangeScene();
        
    }
}
