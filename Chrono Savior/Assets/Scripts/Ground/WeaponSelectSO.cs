using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSelectSO", menuName = "WeaponSelect/WeaponSelectSO", order = 1)]
public class WeaponSelectSO : ScriptableObject
{
    public string weaponName;
    public int tokenCost;
}
