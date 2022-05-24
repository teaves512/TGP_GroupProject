using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuUser : MonoBehaviour
{
    [SerializeField] TMP_Text m_username;
    [SerializeField] TMP_InputField m_usernameInput;
    UserManager m_userManager;

    void Start()
    {
        m_userManager = FindObjectOfType<UserManager>();

        m_username.text = m_userManager.m_User.Name;
        m_usernameInput.text = m_userManager.m_User.Name;
    }

    public void ChangeName()
    {
        m_userManager.UpdateUser(m_usernameInput.text);
        m_username.text = m_usernameInput.text;
    }
}
