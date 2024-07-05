using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWeaponStoreManager : MonoBehaviour
{
   private int coins;
    [SerializeField] Text coinsText;
    [SerializeField] UpgradeTemplateSO[] upgradeItemsSO;
    [SerializeField] UpgradeTemplate[] upgradePanels;
    [SerializeField] Button[] purchaseButtons;
    [SerializeField] Text[] purchaseButtonsText;

    void OnEnable()
    {
        if(StateManagement.Instance != null)
        {
            coins = StateManagement.Instance.GetCoins();
        }
        if(coinsText != null)
        {
            coinsText.text = coins.ToString();
        }
        LoadPanels();
        CheckPurchaseable();
    }


    private void CheckPurchaseable()
    {
        for(int i = 0; i < upgradeItemsSO.Length; i++)
        {
            string name = upgradeItemsSO[i].title;
            bool isUnlocked = false;
            int index = 0;
            if(StateManagement.Instance != null)
            {
                isUnlocked = StateManagement.Instance.IsUnlocked(name);
                index = StateManagement.Instance.GetUpgradeIndex(name);
            }
            if(isUnlocked && coins >= upgradeItemsSO[i].prices[index])
            {
                purchaseButtonsText[i].text = "UPGRADE";
                purchaseButtons[i].interactable = true;
            }
            else if(isUnlocked && coins < upgradeItemsSO[i].prices[index])
            {
                purchaseButtonsText[i].text = "UPGRADE";
                purchaseButtons[i].interactable = false;
            }
            else
            {
                purchaseButtonsText[i].text = "LOCKED";
                purchaseButtons[i].interactable = false;
            }
        }
    
    }
    private void LoadPanels()
    {
        for(int i = 0; i < upgradeItemsSO.Length; i++)
        {
            upgradePanels[i].SetUpgradeItem(upgradeItemsSO[i]);
        }
    }

    public void Upgrade(int index)
    {
        string name = upgradeItemsSO[index].title;
        int upgradeIndex = 0;
        if(StateManagement.Instance != null)
        {
            upgradeIndex = StateManagement.Instance.GetUpgradeIndex(name);
        }
        if(coins >= upgradeItemsSO[index].prices[upgradeIndex])
        {
            coins -= upgradeItemsSO[index].prices[upgradeIndex];
            if(StateManagement.Instance != null)
            {
                StateManagement.Instance.SetCoins(coins);
                StateManagement.Instance.SetUpgradeIndex(name,upgradeIndex + 1);
            }
            coinsText.text = coins.ToString();
            upgradePanels[index].SetUpgradeItem(upgradeItemsSO[index]);
            CheckPurchaseable();
        }
    }
}
