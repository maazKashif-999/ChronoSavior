using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Space/Shield")]


public class Shield : PowerUpEffect
{
    Color playerColor = Color.blue;
    public override void Apply(GameObject target)
    {
        PlayerControls playerControls = target.GetComponent<PlayerControls>();
        if (playerControls != null)
        {
            playerControls.Shield = 0;
            playerControls.Render.color = playerColor;
            playerControls.Invoke("ResetColor", 0.5f);
        }
    }
}
