using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Asteroids")]
public class Aesteroids : PowerUpEffect
{

    [SerializeField] private int damage;

    public override void Apply(GameObject target)
    {
        PlayerControls playerControls = target.GetComponent<PlayerControls>();
        if (playerControls != null)
        {
            playerControls.TakeDamage(damage);
        }
    }

    
}
