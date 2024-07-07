using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementTemplate : MonoBehaviour
{
    [SerializeField] Text titleText;
    [SerializeField] Text descriptionText;

    public void SetAchievement(Achievement achievement)
    {
        if(achievement == null)
        {
            return;
        }
        if(titleText != null)
        {
            titleText.text = achievement.Name;
        }
        if(descriptionText != null)
        {
            descriptionText.text = achievement.Description;
        }
        
    }
}
