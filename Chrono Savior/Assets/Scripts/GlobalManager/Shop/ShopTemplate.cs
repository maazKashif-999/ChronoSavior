using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTemplate : MonoBehaviour
{
    [SerializeField] Text titleText;
    [SerializeField] Text descriptionText;
    [SerializeField] Text priceText;


    public void SetShopItem(ShopItemSO shopItem)
    {
        if(shopItem == null)
        {
            return;
        }
        if(titleText != null)
        {
            titleText.text = shopItem.title;
        }
        if(descriptionText != null)
        {
            descriptionText.text = shopItem.description;
        }
        if(priceText != null)
        {
            if(shopItem.price == 0)
            {
                priceText.text = "Default";
                return;
            }
            else
            {
                priceText.text = "Coins: " + shopItem.price.ToString();
            }
        }
        
        
    }
}
