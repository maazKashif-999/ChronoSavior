using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Space/EnergyMultiplier")]
public class EnergyMultiplier : PowerUpEffect
{
    [SerializeField] private int ratio = 2;
    public override void Apply(GameObject target)
    {
        PlayerControls playerControls = target.GetComponent<PlayerControls>();
        if (playerControls != null)
        {
            // Start the coroutine to apply and reset damage
            playerControls.StartCoroutine(ApplyMultiplier(playerControls));
        }
    }

    private IEnumerator ApplyMultiplier(PlayerControls playerControls)
    {
 
        // Set the new damage
        playerControls.Multi = playerControls.Multiplier * ratio;

        // Wait for 10 seconds
        yield return new WaitForSeconds(10);

        // Reset the damage to the original value
        playerControls.Multi = playerControls.Multiplier;
    }
}
