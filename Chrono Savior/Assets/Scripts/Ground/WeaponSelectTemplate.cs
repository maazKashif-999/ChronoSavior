using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectTemplate : MonoBehaviour
{
    [SerializeField] Text titleText;
    [SerializeField] Text priceText;


    public void SetShopItem(WeaponSelectSO weaponSelect)
    {
        titleText.text = weaponSelect.weaponName;
        priceText.text = "TOKENS: " + weaponSelect.tokenCost.ToString();
    }
}
