using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    // [SerializeField] private AudioSource audioSource;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // if(audioSource != null)
            // {
            //     audioSource.Play();
            // }
            // else
            // {
            //     Debug.LogError("AudioSource not assigned in CoinScript.");
            // }
            Player.Instance.AddCoin();
            if(CoinPoolingAPI.SharedInstance != null)
            {
                CoinPoolingAPI.SharedInstance.Release(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
