using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    public AchievementSO m_Achievement;
    public TextMeshProUGUI m_AchivementName, m_AchievementDescription;

    private Image AchievementImage;

    private void Start()
    {
        AchievementImage = GetComponent<Image>();
        AchievementImage.sprite = m_Achievement.UnlockedImage();
        
    }

    public void OnPointerEnter()
    {
        m_AchivementName.text = m_Achievement.AchievementName;
        m_AchievementDescription.text = m_Achievement.AchievementDescription;

    }
}
