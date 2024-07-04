using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AesteroidPowerup : MonoBehaviour
{
<<<<<<< Updated upstream
    public PowerUpEffect powerUpEffect;
=======
    [SerializeField] private PowerUpEffect powerUpEffect;
    [SerializeField] private GameObject explosionPrefab;
>>>>>>> Stashed changes
    private float screenEdgeX;

    private void Start()
    {
        screenEdgeX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
    }

    void Update()
    {
        transform.position += Vector3.left * powerUpEffect.speed * Time.deltaTime;
        if (transform.position.x < screenEdgeX)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            powerUpEffect.Apply(other.gameObject);
<<<<<<< Updated upstream
            Destroy(gameObject); 
=======
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
            gameObject.SetActive(false);
>>>>>>> Stashed changes
        }
        else if (other.CompareTag("PlayerBullets") || other.CompareTag("EnemyBullets"))
        {
            Destroy(other.gameObject);
        }
    }
}
