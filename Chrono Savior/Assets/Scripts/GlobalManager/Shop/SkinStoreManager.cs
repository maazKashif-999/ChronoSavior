using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkinStoreManager : MonoBehaviour
{
    private int coins;
    bool redSkinEquipped = false;
    [SerializeField] Text coinsText;
    [SerializeField] ShopItemSO[] shopItemsSO;
    [SerializeField] ShopTemplate[] shopPanels;
    [SerializeField] Button[] purchaseButtons;
    [SerializeField] Text[] purchaseButtonsText;
    [SerializeField] Text equippedSkinName;
    [SerializeField] Image blueSkin;
    [SerializeField] Image redSkin;
    private int redSkinIndex = 1;
    private int blueSkinIndex = 0;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(StateManagement.Instance != null)
        {
            coins = StateManagement.Instance.GetCoins();
            redSkinEquipped = StateManagement.Instance.IsRedSkinEquipped();
        }
        if(coinsText != null)
        {
            coinsText.text = coins.ToString();
        }
        SetSkinDisplay();
        LoadPanels();
        CheckPurchaseable();
    }

    // Update is called once per frame
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
                purchaseButtonsText[i].text = "SELECT";
                purchaseButtons[i].interactable = true;
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

    private void SetSkinDisplay()
    {
        if(blueSkin != null && redSkin != null)
        {
            if(redSkinEquipped)
            {
                blueSkin.gameObject.SetActive(false);
                redSkin.gameObject.SetActive(true);
                equippedSkinName.text = shopItemsSO[redSkinIndex].title;
            }
            else
            {
                blueSkin.gameObject.SetActive(true);
                redSkin.gameObject.SetActive(false);
                equippedSkinName.text = shopItemsSO[blueSkinIndex].title;

            }
        }
    }

    public void HandleBlueSkinClick()
    {
        if(!redSkinEquipped) return;
        
        redSkinEquipped = false;
        SetSkinDisplay();
        if(StateManagement.Instance != null)
        {
            StateManagement.Instance.SetBlueSkin();
        }
    }

    private void SelectRedSkin()
    {
        redSkinEquipped = true;
        SetSkinDisplay();
        if(StateManagement.Instance != null)
        {
            StateManagement.Instance.SetRedSkin();
        }
    }

    private void PurchaseRedSkin()
    {
        if(StateManagement.Instance != null)
        {
            StateManagement.Instance.UnlockRedSkin();
            coins -= shopItemsSO[redSkinIndex].price;
            StateManagement.Instance.SetCoins(coins);
        }
        if(coinsText != null)
        {
            coinsText.text = coins.ToString();
        }
        CheckPurchaseable();
    }

    public void HandleRedSkinClick()
    {
        if(redSkinEquipped) return;
        
        if(StateManagement.Instance != null)
        {
            if(StateManagement.Instance.IsUnlocked(shopItemsSO[redSkinIndex].title))
            {
                SelectRedSkin();
            }
            else if(coins >= shopItemsSO[redSkinIndex].price)
            {
                PurchaseRedSkin();
                SelectRedSkin();
            }
        }
    }

}
