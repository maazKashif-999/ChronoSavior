using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectTemplate : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text priceText;

    private void Start()
    {
        if (titleText == null)
        {
            Debug.LogError("TitleText is not assigned in the inspector.");
        }

        if (priceText == null)
        {
            Debug.LogError("PriceText is not assigned in the inspector.");
        }
    }

    public void SetShopItem(WeaponSelectSO weaponSelect)
    {
        if (weaponSelect == null)
        {
            Debug.LogError("WeaponSelectSO is null in SetShopItem.");
            return;
        }

        if (titleText != null)
        {
            titleText.text = weaponSelect.weaponName;
        }
        else
        {
            Debug.LogError("TitleText is null in SetShopItem.");
        }

        if (priceText != null)
        {
            priceText.text = "TOKENS: " + weaponSelect.tokenCost.ToString();
        }
        else
        {
            Debug.LogError("PriceText is null in SetShopItem.");
        }
    }
}
