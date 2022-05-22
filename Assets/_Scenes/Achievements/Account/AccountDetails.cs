using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AccountDetails 
{

    
    [SerializeField]private string UserName;
    [SerializeField] private AchievementSavable UserAchievements;
    [SerializeField]private bool hasRegistered;

    
    public bool HasRegistered { get => hasRegistered;}
    public AchievementSavable PlayersAchievements => UserAchievements;

    public string Name
    {
        get => UserName;
        set => UserName = value;
    }


    
    public void Initialize()
    {
        if (!hasRegistered)
        {

            UserAchievements = new AchievementSavable();
            hasRegistered = true;
        }
        
    }
        
  
}
