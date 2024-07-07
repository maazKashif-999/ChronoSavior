using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    public List<Achievement> allAchievements;

    private List<Achievement> locked;
    private List<Achievement> unlocked;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAchievements();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAchievements()
    {
        locked = new List<Achievement>();
        unlocked = new List<Achievement>();

        foreach (Achievement ach in allAchievements)
        {
            if (PlayerPrefs.HasKey(ach.Name))
            {
                string json = PlayerPrefs.GetString(ach.Name);
                Achievement loadedAchievement = ScriptableObject.CreateInstance<Achievement>();
                loadedAchievement.FromJson(json);

                if (loadedAchievement.Unlocked)
                {
                    unlocked.Add(loadedAchievement);
                }
                else
                {
                    locked.Add(loadedAchievement);
                }
            }
            else
            {
                // Create a copy of the achievement so that original ScriptableObject is not modified directly.
                Achievement newAchievement = ScriptableObject.CreateInstance<Achievement>();
                JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(ach), newAchievement);

                locked.Add(newAchievement);
                SaveAchievements(newAchievement);
            }
        }
        PrintAchievements();
    }

    public void SaveAchievements(Achievement achievement)
    {
        PlayerPrefs.SetString(achievement.Name, achievement.ToJson());
        PlayerPrefs.Save();
    }

    public void CheckLocked(string name)
    {
        foreach (Achievement ab in locked)
        {
            if (ab.Name == name)
            {
                ab.Unlocked = true;
                unlocked.Add(ab);
                locked.Remove(ab);
                SaveAchievements(ab);
                Debug.Log("Achievement unlocked: " + ab.Name);
                break;
            }
        }
    }

    public void PrintAchievements()
    {
        Debug.Log("Locked Achievements:");
        foreach (Achievement ach in locked)
        {
            Debug.Log($"Name: {ach.Name}, Description: {ach.Description}, Unlocked: {ach.Unlocked}");
        }

        Debug.Log("Unlocked Achievements:");
        foreach (Achievement ach in unlocked)
        {
            Debug.Log($"Name: {ach.Name}, Description: {ach.Description}, Unlocked: {ach.Unlocked}");
        }
    }
}
