using System.Collections;
using System.Collections.Generic;
using UnityEditor.VisionOS;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStoreManager : MonoBehaviour
{
    private int coins;
    [SerializeField] Text coinsText;
    [SerializeField] ShopItemSO[] shopItemsSO;
    [SerializeField] ShopTemplate[] shopPanels;
    [SerializeField] Button[] purchaseButtons;
    [SerializeField] Text[] purchaseButtonsText;
    private int arIndex = 3;
    private int shotgunIndex = 4;
    // Start is called before the first frame update
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
        for(int i = 0; i < shopItemsSO.Length; i++)
        {
            string name = shopItemsSO[i].title;
            bool isUnlocked = false;
            if(StateManagement.Instance != null)
            {
                isUnlocked = StateManagement.Instance.IsUnlocked(name);
            }
            if(isUnlocked)
            {
                purchaseButtonsText[i].text = "UNLOCKED";
                purchaseButtons[i].interactable = false;
            }
            else if(!isUnlocked && coins >= shopItemsSO[i].price)
            {
                purchaseButtonsText[i].text = "PURCHASE";
                purchaseButtons[i].interactable = true;
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
        for(int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanels[i].SetShopItem(shopItemsSO[i]);
        }
    }

    public void PurchaseAR()
    {
        if(StateManagement.Instance != null)
        {
            StateManagement.Instance.UnlockAR();
            coins -= shopItemsSO[arIndex].price;
            StateManagement.Instance.SetCoins(coins);
            if(coinsText != null)
            {
                coinsText.text = coins.ToString();
            }
            CheckPurchaseable();
        }
    }

    public void PurchaseShotgun()
    {
        if(StateManagement.Instance != null)
        {
            StateManagement.Instance.UnlockShotgun();
            coins -= shopItemsSO[shotgunIndex].price;
            StateManagement.Instance.SetCoins(coins);
            if(coinsText != null)
            {
                coinsText.text = coins.ToString();
            }
            CheckPurchaseable();
        }
    }
}
