using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeTemplate", menuName = "Shop/UpgradeTemplate")]
public class UpgradeTemplateSO : ScriptableObject
{
    public string title;
    public int[] prices = {20,22,26,32,40};

}
