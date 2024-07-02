using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PowerUps/OneShotOneKill")]
public class OneShotOneKill : PowerUp
{
    public override void UsePowerUp(GameObject player)
    {
        Player.Instance.OneShotOneKill();
    }

}

