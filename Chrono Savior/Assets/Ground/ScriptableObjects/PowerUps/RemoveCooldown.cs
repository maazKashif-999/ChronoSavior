using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/RemoveCooldown")]
public class RemoveCooldown : PowerUp
{
    public override void UsePowerUp(GameObject player)
    {
        Player.Instance.RemoveCooldown();
    }
}
