using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AchievementManagerScript : MonoBehaviour
{
    [SerializeField] AchievementTemplate[] achievementTemplates;
    private List<Achievement> achievements = new List<Achievement>();
    [SerializeField] private bool isUnlocked;

    void OnEnable()
    {
        if(AchievementManager.Instance != null)
        {
            if(isUnlocked)
            {
                achievements = AchievementManager.Instance.Unlocked;
            }
            else
            {
                achievements = AchievementManager.Instance.Locked;
            }
        }
        Disable();
        LoadPanels();
    }
    
    private void Disable()
    {
        for(int i = 0; i < achievementTemplates.Length; i++)
        {
            achievementTemplates[i].gameObject.SetActive(false);
        }
    }
    private void LoadPanels()
    {
        for(int i = 0; i < achievements.Count; i++)
        {
            achievementTemplates[i].gameObject.SetActive(true);
            achievementTemplates[i].SetAchievement(achievements[i]);
        }
    }


}
