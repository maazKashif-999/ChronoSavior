using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AesteroidPowerup : MonoBehaviour
{
    [SerializeField] private PowerUpEffect powerUpEffect;
    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private string prefabTag;
    private float screenEdgeX;

    private void Start()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            screenEdgeX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        }
        else
        {
            Debug.LogWarning("Main camera is not assigned.");
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
        if (other == null)
            return;

        if (other.CompareTag("Player"))
        {
            if (powerUpEffect != null)
            {
                powerUpEffect.Apply(other.gameObject);
            }
            else
            {
                Debug.LogWarning("PowerUpEffect is not assigned.");
            }

            if (PoolManager.Instance != null)
            {
                PoolManager.Instance.SpawnFromPool(prefabTag, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("PoolManager instance is null.");
            }

            if (PlayerControls.Instance != null && !gameObject.CompareTag("coins") && !gameObject.CompareTag("token"))
            {
                PlayerControls.Instance.PlayExplosionSound();
            }
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("PlayerBullets") || other.CompareTag("EnemyBullets"))
        {
            other.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.ReturnToPool(gameObject.tag, gameObject);
        }
    }

    private void OnDestroy()
    {
        //Debug.Log("Aesteroid destroyed");
    }
}
