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
        titleText.text = shopItem.title;
        descriptionText.text = shopItem.description;
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
