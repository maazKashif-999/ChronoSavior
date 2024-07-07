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
        titleText.text = achievement.Name;
        descriptionText.text = achievement.Description;
    }
}
