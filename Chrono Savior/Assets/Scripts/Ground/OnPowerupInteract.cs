using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPowerupInteract : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PowerUp powerup;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other != null)
        {
            if(other.CompareTag("Player"))
            {
                powerup.UsePowerUp(other.gameObject);
                Destroy(gameObject);
            }
        }
    }

}
