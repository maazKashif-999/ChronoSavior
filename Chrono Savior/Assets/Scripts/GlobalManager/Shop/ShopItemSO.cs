using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Shop/ShopItem")]
public class ShopItemSO : ScriptableObject
{
    public string title;
    public string description;
    public int price;
}
