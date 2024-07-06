using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Space/Damage")]
public class DamageEffect : PowerUpEffect
{
    [SerializeField] private int ratio;
     Color playerColor = Color.black;
    public override void Apply(GameObject target)
    {
        PlayerControls playerControls = target.GetComponent<PlayerControls>();
        if (playerControls != null)
        {
            // Start the coroutine to apply and reset damage
            playerControls.StartCoroutine(ApplyOneShotDamage(playerControls));
        }
    }

    private IEnumerator ApplyOneShotDamage(PlayerControls playerControls)
    {
 
        // Set the new damage
        playerControls.Damage = playerControls.Damage * ratio;
        playerControls.Render.color = playerColor;
        // Wait for 10 seconds
        yield return new WaitForSeconds(10);
        playerControls.ResetColor();
        // Reset the damage to the original value
        playerControls.Damage = playerControls.MaxDamage;
    }
}
