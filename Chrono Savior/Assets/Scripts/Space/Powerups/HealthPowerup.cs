using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Space/Health")]

public class Health : PowerUpEffect
{
    Color playerColor = new Color(1f, 0.5f, 0f);
    public override void Apply(GameObject target)
    {
        PlayerControls playerControls = target.GetComponent<PlayerControls>();
        if (playerControls != null)
        {
            playerControls.Health = 0;
            playerControls.Render.color = playerColor;
            playerControls.Invoke("ResetColor", 0.5f);
        }

    }
}
