using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShooter : MonoBehaviour, IEnemy
{
    // Start is called before the first frame update
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private GameObject active;
    [SerializeField] private GameObject idle;
    [SerializeField] private GameObject deathAnimation;
    private const float MAX_HEALTH = 50f;
    private float currentHealth;
    private Player player;
    private float timer;
    private const float TIME_BETWEEN_SHOTS = 1.5f;
    private const float DETECT_DISTANCE = 5f;
    
    void Start() 
    {
        player = Player.Instance;
        currentHealth = MAX_HEALTH;
        idle.SetActive(true);
        active.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (player.AreEnemiesFrozen())
        {
            return;
        }
        Vector2 distanceVec = player.transform.position - transform.position;
        float distanceSquared = distanceVec.sqrMagnitude;
        if(distanceSquared < (DETECT_DISTANCE * DETECT_DISTANCE))
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
        Destroy(gameObject);
    }

}
