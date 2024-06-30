using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AesteroidPowerup : MonoBehaviour
{
    [SerializeField] private PowerUpEffect powerUpEffect;
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
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            powerUpEffect.Apply(other.gameObject);
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("PlayerBullets") || other.CompareTag("EnemyBullets"))
        {
            other.gameObject.SetActive(false);
        }
    }
    private void OnDisable() {
        Debug.Log("Aesteroid destroyed");
        PoolManager.Instance.ReturnToPool("Aesteroid", gameObject);
    }    
}
