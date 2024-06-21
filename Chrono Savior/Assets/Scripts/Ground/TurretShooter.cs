using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShooter : MonoBehaviour, IEnemy
{
    // Start is called before the first frame update
    private const float MAX_HEALTH = 50f;
    private float currentHealth;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private GameObject active;
    [SerializeField] private GameObject idle;
    [SerializeField] private GameObject deathAnimation;
    private GameObject player;
    private float timer;
    private const float TIME_BETWEEN_SHOTS = 1.5f;
    private const float DETECT_DISTANCE = 5f;
    
    void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = MAX_HEALTH;
        idle.SetActive(true);
        active.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.Instance.AreEnemiesFrozen())
        {
            return;
        }
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if(distance < DETECT_DISTANCE)
        {
            idle.SetActive(false);
            active.SetActive(true);
            timer += Time.deltaTime;
            if(timer > TIME_BETWEEN_SHOTS)
            {
                Shoot();
                timer = 0;
            }
        }
        else
        {
            idle.SetActive(true);
            active.SetActive(false);
        }
    }

    void Shoot()
    {
        Instantiate(bullet, bulletPosition.position, Quaternion.identity);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Instantiate(deathAnimation, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

}
