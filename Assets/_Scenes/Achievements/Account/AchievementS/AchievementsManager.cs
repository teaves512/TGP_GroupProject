using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementsManager : MonoBehaviour
{
    [Header("Achievements")]
    [SerializeField]private AchievementThreshold BombsAchievements; 
    [SerializeField]private AchievementThreshold DistanceAchievements; 
    [SerializeField]private AchievementThreshold WallDestroyedAchievements; 
    [SerializeField]private AchievementThreshold PlayersKilledAchievements;
    private List<AchievementThreshold> AchievmentThresholds = new List<AchievementThreshold>();
    UserManager userManager;

    [Header("UI")] 
    [SerializeField] private GameObject ScrollPanel;
    [SerializeField] private TextMeshProUGUI m_AchivementName, m_AchievementDescription;
    [SerializeField] private GameObject ImageTemplate;
    public List<GameObject> AchievementImages = new List<GameObject>();



    private void Start()
    {
        userManager = FindObjectOfType<UserManager>();

        AddAchievementToList();
        foreach (var achievment in AchievmentThresholds)
        {
            achievment.CheckAchievement();
        }

        foreach (var achievment in AchievmentThresholds)
        {
            for (int i = 0; i < achievment.AchievementAmount; i++)
            {
                GameObject Image = Instantiate(ImageTemplate, ScrollPanel.transform);
                Image.GetComponent<AchievementUI>().m_Achievement = achievment.achievementSOs[i];
                Image.GetComponent<AchievementUI>().m_AchivementName = m_AchivementName;
                Image.GetComponent<AchievementUI>().m_AchievementDescription = m_AchievementDescription;
                AchievementImages.Add(Image);
                
            }
        }
    }

    void AddAchievementToList()
    {
        BombsAchievements.currentAchievementValue = userManager.m_User.PlayersAchievements.Bombs;
        
        AchievmentThresholds.Add(BombsAchievements);

        DistanceAchievements.currentAchievementValue = (int)userManager.m_User.PlayersAchievements.Distance;
        
        AchievmentThresholds.Add(DistanceAchievements);

        WallDestroyedAchievements.currentAchievementValue =
            userManager.m_User.PlayersAchievements.ObjectsDestroyed;
        
        AchievmentThresholds.Add(WallDestroyedAchievements);

        PlayersKilledAchievements.currentAchievementValue =
            userManager.m_User.PlayersAchievements.EnemiesSpliffed;
        
        AchievmentThresholds.Add(PlayersKilledAchievements);
    }
    
    
}
