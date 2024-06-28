using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Space/Coin")]
public class Coin : PowerUpEffect
{
    public override void Apply(GameObject target)
    {
        PlayerControls playerControls = target.GetComponent<PlayerControls>();
        if (playerControls != null)
        {
            playerControls.UpdateCoin();
        }
    }
}
