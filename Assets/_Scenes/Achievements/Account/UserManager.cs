using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// THIS CLASS HOLD THE ACTIONS THAT CAN BE DONT TO THE USER ACCOUNT AND ALSO INITIALIZES IT and Holds it
/// </summary>
public class UserManager : MonoBehaviour
{
    public AccountDetails m_User { get; private set; }
    private string m_Path;
    private string file;

    private void Awake()
    {
        file = "RegisteredUser";
        m_Path = Application.persistentDataPath + "/" + file;

        Load();
    }

    public void CreateUser()
    {
        
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
        Save();
    }
    
    public void Save()
    {
        string json = JsonUtility.ToJson(m_User);
        WriteToFile(json);
        Debug.Log("Save (PM) , " + m_User.Name);
    }

    public void Load()
    {
        m_User = new AccountDetails();
        string json = ReadFromFile();

        if (json != null)
        {
            JsonUtility.FromJsonOverwrite(json, m_User);
            Debug.Log("Loaded  (PM) , " + m_User.Name + m_User.HasRegistered);
        }
        else
        {
            m_User.Initialize();
            m_User.Name = System.Environment.UserName;
            Save();
        }
    }
    
    
    
    private void WriteToFile(string json)// responsible for storing the data into the jsonfile
    {

        FileStream fileStream = new FileStream(m_Path, FileMode.Create);// this will create a file at the given path
        using (StreamWriter streamWriter = new StreamWriter(fileStream))//using this filestream we will initialize the streamwriter and it will then write the json string that we took as a parameter
        {
            streamWriter.Write(json);// it will create a file and save those values into the file
        }
        
    }
    
    private string ReadFromFile()// this will read the data present in the json file 
    {

        if (File.Exists(m_Path))
        {
            using (StreamReader reader = new StreamReader(m_Path))
            {
                string filedata = reader.ReadToEnd();
                return filedata;
            }
        }
        else
        {
            Debug.Log("File path doesn't exist");
            return null;
        }
            
        return null;
    }
   
    
}
