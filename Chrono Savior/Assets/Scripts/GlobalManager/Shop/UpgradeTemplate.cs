using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTemplate : MonoBehaviour
{
    [SerializeField] Text titleText;
    [SerializeField] Text upgradeCountText;
    [SerializeField] Text priceText;

    public void SetUpgradeItem(UpgradeTemplateSO upgradeItem)
    {
        if(upgradeItem == null)
        {
            return;
        }
        if(titleText != null)
        {
            titleText.text = upgradeItem.title;
        }
        if(upgradeCountText != null && priceText != null)
        {
            if(StateManagement.Instance != null)
            {
                int index = StateManagement.Instance.GetUpgradeIndex(upgradeItem.title);
                upgradeCountText.text = "Upgrade Status : " + (index) + "/5";
                priceText.text = "Coins: " + upgradeItem.prices[index].ToString();
            }
            else
            {
                upgradeCountText.text = "Upgrade Status: 0/5";
                priceText.text = "Coins: " + upgradeItem.prices[0].ToString();
            }
        }
        
        
        
        
    }
}
