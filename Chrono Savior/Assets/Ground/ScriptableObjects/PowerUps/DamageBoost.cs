using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PowerUps/DamageBoost")]
public class DamageBoost : PowerUp
{
    public override void UsePowerUp(GameObject player)
    {
        Player.Instance.DamageBoost();
    }
}