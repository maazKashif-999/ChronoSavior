using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievements/Achievement")]
public class Achievement : ScriptableObject
{
    [SerializeField] private string achievementName;
    [SerializeField] private string description;
    [SerializeField] private bool isUnlocked;
    public string Name
    {
        get { return achievementName; }
    }
    public string Description
    {
        get { return description; }
    }
    public bool Unlocked
    {
        get { return isUnlocked; }
        set { isUnlocked = value; }
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void FromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
    
}
