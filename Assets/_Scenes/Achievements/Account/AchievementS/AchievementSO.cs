using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement")]
public class AchievementSO : ScriptableObject
{
    public string AchievementName;
    [TextArea] public string AchievementDescription;
    public float threshold;
    public Sprite BadgeImage;
    public Sprite HiddenBadgeImage;
    protected bool unlocked = false;

    public Sprite UnlockedImage()
    {
        if (unlocked)
        {
            return BadgeImage;
        }
        else
        {
            return HiddenBadgeImage;
        }
    }

    public void Unlock()
    {
        if (!unlocked)
        {
            unlocked = true;
        }
    }
    
    public void Lock()
    {
        if(unlocked)
        {
            unlocked = false;
        }
    }
}
