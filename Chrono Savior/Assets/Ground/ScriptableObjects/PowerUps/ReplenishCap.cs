using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/ReplenishCap")]
public class ReplenishCap : PowerUp
{
    private WeaponSwitch weaponManager;
    public override void UsePowerUp(GameObject player)
    {
        weaponManager = FindObjectOfType<WeaponSwitch>();
        for (int i = 0; i < weaponManager.transform.childCount; i++)
        {
            weaponManager.transform.GetChild(i).gameObject.GetComponent<GunShoot>().ReplenishAmmo();
        }
    }

}
 