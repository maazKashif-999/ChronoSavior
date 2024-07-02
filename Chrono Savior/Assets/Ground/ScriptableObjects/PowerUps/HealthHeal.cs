using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/HealthHeal")]
public class HealthHeal : PowerUp
{
    public override void UsePowerUp(GameObject other)
    {
        Player.Instance.FullHealthHeal();
    }

}
