using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// THIS CLASS HOLD THE ACTIONS THAT CAN BE DONT TO THE USER ACCOUNT AND ALSO INITIALIZES IT and Holds it
/// </summary>
public class UserManager : SingletonPersistence<UserManager>
{
    public AccountDetails m_User;
    private UserManager m_UserManager;
    private string m_Path;
    private string file;

    public void CreateUser()
    {
        file = "RegisteredUser";
        m_User = new AccountDetails();
        m_User.Initialize();
    }

    public void UpdateUser(string _name = default)
    {
        if (m_User == null)
        {
            CreateUser();
        }
        m_User.Name = _name;
        m_User.Save();
    }
    
   
    
}
