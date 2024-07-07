using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Space/Shield")]


public class Shield : PowerUpEffect
{
    public override void Apply(GameObject target)
    {
        PlayerControls playerControls = target.GetComponent<PlayerControls>();
        if (playerControls != null)
        {
            playerControls.Shield = 0;
        }
    }
}
