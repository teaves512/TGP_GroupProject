using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AccountDetails 
{

    
    [SerializeField]private string UserName;
    [SerializeField] private AchievementSavable UserAchievements;
    [SerializeField]private bool hasRegistered;
    private string m_Path;
    private string file;
    
    public bool HasRegistered { get => hasRegistered;}
    public AchievementSavable PlayersAchievements => UserAchievements;

    public string Name
    {
        get => UserName;
        set => UserName = value;
    }

    public AccountDetails()
    {
        Initialize();
    }
    
    public void Initialize()
    {
        if (!hasRegistered)
        {
            file = "RegisteredUser";
            m_Path = Application.persistentDataPath + "/" + file;
            UserAchievements = new AchievementSavable();
            hasRegistered = true;
        }
        
    }
        
    public void Save()
    {
        string json = JsonUtility.ToJson(this);
        WriteToFile(json);

    }

    public void Load()
    {
        string json = ReadFromFile();
        JsonUtility.FromJsonOverwrite(json, this);
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
