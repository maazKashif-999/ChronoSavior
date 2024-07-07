using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // if (audioSource != null)
            // {
            //     audioSource.Play();
            // }
            // else
            // {
            //     Debug.LogError("AudioSource not assigned in CoinScript.");
            // }

            Player player = Player.Instance;
            if (player != null)
            {
                player.AddCoin();
            }
            else
            {
                Debug.LogError("Player instance is null in CoinScript.");
            }

            if (CoinPoolingAPI.SharedInstance != null)
            {
                CoinPoolingAPI.SharedInstance.Release(this);
            }
            else
            {
                Debug.LogWarning("CoinPoolingAPI instance is null, destroying coin.");
                Destroy(gameObject);
            }
        }
    }
}
