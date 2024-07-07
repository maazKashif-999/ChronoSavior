using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;
    [SerializeField] private List<GameObject> notificationPrefabs; 
    private Queue<GameObject> queue;
    [SerializeField] private List<Achievement> allAchievements;
    private List<Achievement> locked;
    private List<Achievement> unlocked;

    public List<Achievement> Locked
    {
        get { return locked; }
    }

    public List<Achievement> Unlocked
    {
        get { return unlocked; }
    }
    private int counter;

    public Vector3 min;
    public Vector3 max;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAchievements();
            Camera mainCamera = Camera.main;
            counter = 0;
            if (mainCamera != null)
            {
                min = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
                max = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
            }
            queue = new Queue<GameObject>();
            foreach (GameObject obj in notificationPrefabs){
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
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
                Achievement newAchievement = ScriptableObject.CreateInstance<Achievement>();
                JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(ach), newAchievement);

                locked.Add(newAchievement);
                SaveAchievements(newAchievement);
            }
        }
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
                DisplayAchievement(ab);
                Debug.Log("Achievement unlocked: " + ab.Name);
                break;
            }
        }
    }

    private void DisplayAchievement(Achievement achievement)
    {
        notificationPrefabs[counter].SetActive(true);
        NotificationController notificationController = notificationPrefabs[counter].GetComponent<NotificationController>();
        notificationController.SetMessage($"Achievement Unlocked \n{achievement.Name}"); // Set text content

        StartCoroutine(DisplayAchievementCoroutine(notificationPrefabs[counter]));
        counter++;
        
    }

    private IEnumerator DisplayAchievementCoroutine(GameObject instance)
    {
        yield return new WaitForSeconds(5.0f);
        instance.SetActive(false);
        counter--;
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
