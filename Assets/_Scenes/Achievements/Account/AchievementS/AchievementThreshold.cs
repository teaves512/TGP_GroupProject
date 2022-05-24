using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AchievementThreshold
{
    public AchievementSO[] achievementSOs;
    public int currentAchievementValue = 0;
    public virtual void CheckAchievement()
    {
        
        foreach (var achievement in achievementSOs)
        {
            if (achievement.threshold <= currentAchievementValue)
            {
                achievement.Unlock();
            }
            else
                achievement.Lock();
        }
    }
    
    public int AchievementAmount => achievementSOs.Length;
}
