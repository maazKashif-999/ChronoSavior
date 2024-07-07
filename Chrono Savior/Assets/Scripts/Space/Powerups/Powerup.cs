using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip powerUpSound; 
    [SerializeField] private PowerUpEffect powerUpEffect;
    private float screenEdgeX;

    private void Start()
    {
        screenEdgeX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on the power-up object.");
        }
        if (powerUpSound == null)
        {
            Debug.LogError("No powerUpSound assigned in the inspector.");
        }

    }
    void Update()
    {
        transform.position += Vector3.left * powerUpEffect.speed * Time.deltaTime;
        if (transform.position.x < screenEdgeX)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            powerUpEffect.Apply(other.gameObject);

            if (audioSource != null && powerUpSound != null)
            {
                audioSource.PlayOneShot(powerUpSound);  
            }

            gameObject.SetActive(false);
        }
    }
    private void OnDisable() {
        PoolManager.Instance.ReturnToPool(gameObject.tag, gameObject);
    }
    private void OnDestroy() {
        Debug.Log("Powerup Destroyed");
    }
}
