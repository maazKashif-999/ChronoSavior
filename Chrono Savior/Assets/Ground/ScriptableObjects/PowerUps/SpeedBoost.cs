using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PowerUps/SpeedBoost")]
public class SpeedBoost : PowerUp
{
    public override void UsePowerUp(GameObject player)
    {
        Player.Instance.SpeedBoost();
    }
}
