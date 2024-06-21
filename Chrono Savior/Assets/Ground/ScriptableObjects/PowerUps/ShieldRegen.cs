using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/ShieldRegen")]
public class ShieldRegen : PowerUp
{
    public override void UsePowerUp(GameObject player)
    {
        Player.Instance.FullShieldHeal();
    }
}
