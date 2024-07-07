using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManagement : MonoBehaviour
{
    public static StateManagement Instance;

    private const string GROUND_HIGH_SCORE = "GroundHighScore";
    private const string SPACE_HIGH_SCORE = "SpaceHighScore";
    private const string SPACE_KILL_COUNT = "SpaceKillCount";
    private const string GROUND_KILL_COUNT = "GroundKillCount";
    private const string COINS = "Coins"; 
    private const string TOKENS = "Tokens";
    private const string CRIMSON_ARMOR_EQUIPPED = "RedSkinEquipped";
    private const string CRIMSON_ARMOR = "Crimson Armor";
    private const string AQUA_ARMOR = "Aqua Armor";
    private const string SMG = "SMG";
    private const string PISTOL = "Pistol";
    private const string SNIPER = "Sniper";
    private const string AR = "AR";
    private const string SHOTGUN = "Shotgun";
    private const string SESSION_LOAD = "SessionLoad";

    
    void Awake()
    {
        
        if(Instance == null)
        {
            Instance = this;
            SetDefaultState();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    
    }
    private void SetDefaultState()
    {
        PlayerPrefs.SetInt(AQUA_ARMOR,1);
        PlayerPrefs.SetInt(SMG,1);
        PlayerPrefs.SetInt(PISTOL,1);
        PlayerPrefs.SetInt(SNIPER,1);
    }
    public void SetGroundHighestScore(float highscore)
    {
        PlayerPrefs.SetFloat(GROUND_HIGH_SCORE, highscore);
    }

    public float GetGroundHighestScore()
    {
        return PlayerPrefs.GetFloat(GROUND_HIGH_SCORE,0);
    }

    public void SetSpaceHighestScore(float highscore)
    {
        PlayerPrefs.SetFloat(SPACE_HIGH_SCORE, highscore);
    }

    public float GetSpaceHighestScore()
    {
        return PlayerPrefs.GetFloat(SPACE_HIGH_SCORE,0);
    }

    public int GetSpaceKillCount()
    {
        return PlayerPrefs.GetInt(SPACE_KILL_COUNT,0);
    }
    public void SetSpaceKillCount(int count)
    {
        PlayerPrefs.SetInt(SPACE_KILL_COUNT, count);
    }

    public int GetGroundKillCount()
    {
        return PlayerPrefs.GetInt(GROUND_KILL_COUNT,0);
    }
    public void SetGroundKillCount(int count)
    {
        PlayerPrefs.SetInt(GROUND_KILL_COUNT, count);
    }

    public void SetCoins(int coins)
    {
        PlayerPrefs.SetInt(COINS, coins);
    }

    public int GetCoins()
    {
        return PlayerPrefs.GetInt(COINS,0);
    }

    public void SetTokens(int tokens)
    {
        PlayerPrefs.SetInt(TOKENS, tokens);
    }

    public int GetTokens()
    {
        return PlayerPrefs.GetInt(TOKENS,0);
    }

    public bool IsUnlocked(string name)
    {
        return PlayerPrefs.GetInt(name,0) == 1;
    }

    public void UnlockRedSkin()
    {
        PlayerPrefs.SetInt(CRIMSON_ARMOR,1);
    }
    public bool IsRedSkinEquipped()
    {
        return PlayerPrefs.GetInt(CRIMSON_ARMOR_EQUIPPED,0) == 1;
    }

    public void SetBlueSkin()
    {
        PlayerPrefs.SetInt(CRIMSON_ARMOR_EQUIPPED,0);
    }

    public void SetRedSkin()
    {
        PlayerPrefs.SetInt(CRIMSON_ARMOR_EQUIPPED,1);
    }

    public void UnlockAR()
    {
        PlayerPrefs.SetInt(AR,1);
    }

    public void UnlockShotgun()
    {
        PlayerPrefs.SetInt(SHOTGUN,1);
    }

    public int GetUpgradeIndex(string name)
    {
        string upgradeIndex = name + "Index";
        return PlayerPrefs.GetInt(upgradeIndex,0);
    }

    public void SetUpgradeIndex(string name, int index)
    {
        string upgradeIndex = name + "Index";
        int updatedIndex = Mathf.Clamp(index,0,5);
        PlayerPrefs.SetInt(upgradeIndex,updatedIndex);
    }

    public void SetSessionLoad(bool load)
    {
        PlayerPrefs.SetInt(SESSION_LOAD,load ? 1 : 0);
    }

    public bool CanLoadSession()
    {
        return PlayerPrefs.GetInt(SESSION_LOAD,0) == 1;
    }

}
