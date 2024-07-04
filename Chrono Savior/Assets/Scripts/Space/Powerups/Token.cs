using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Space/Token")]
public class Token : PowerUpEffect
{
    public override void Apply(GameObject target)
    {
        PlayerControls playerControls = target.GetComponent<PlayerControls>();
        if (playerControls != null)
        {
            playerControls.UpdateToken();
        }
    }
}
