//using UnityEngine;
//using System.Collections.Generic;

//public class EnemyShip : MonoBehaviour
//{
//    private SpriteRenderer spriteRenderer;
//    private Color originalColor;
//    protected float speed; // Speed of the enemy ship
//    protected float limitXPosition; // X position where the ship stops
//    [SerializeField] private GameObject tokenPrefab;
//    [SerializeField] private List<string> powerupPrefabs;
//    protected float fireInterval; // Interval between consecutive bullet fires
//    protected float bulletSpeed; // Speed of the fired bullets
//    protected int health;

//    protected int MAX_HEALTH;
//    protected int damage;
//    protected string bulletTag;

//    private EnemyWaveManager enemyWaveManager;
//    protected float angle;
//    protected float coinDroppingProbability;
//    protected float tokenDroppingProbability;

//    protected float powerUpDroppingProbability;

//    private PlayerControls player;
//    protected float nextFireTime; // Time when the ship can fire next
//    private void OnEnable() {
//        health = MAX_HEALTH;
//    }
//    protected virtual void Start()
//    {
//        nextFireTime = Time.time;
//        transform.rotation = Quaternion.Euler(0, 0, 90);

//        enemyWaveManager = FindObjectOfType<EnemyWaveManager>();
//        player = FindObjectOfType<PlayerControls>();

//        spriteRenderer = GetComponent<SpriteRenderer>();
//        if (spriteRenderer == null)
//        {
//            Debug.Log("Missing SpriteRenderer component on " + gameObject.name);
//            return;
//        }
//        originalColor = spriteRenderer.color;
//    }

//    protected virtual void Update()
//    {
//        if (transform.position.x >= limitXPosition)
//        {
//            transform.position += Vector3.left * speed * Time.deltaTime;
//        }
//        else
//        {
//            if (Time.time > nextFireTime)
//            {
//                FireBullet();
//                nextFireTime = Time.time + fireInterval;
//            }
//        }
//    }

//    protected virtual void FireBullet()
//    {
//        if (PoolManager.Instance != null)
//        {
//            GameObject bullet = PoolManager.Instance.SpawnFromPool(bulletTag, transform.position, Quaternion.identity);
//            PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();
//            if (bulletScript != null && player != null)
//            {
//                Quaternion rotation = Quaternion.Euler(0, 0, angle);
//                bulletScript.Initialize(player.transform.position, bulletSpeed, damage, rotation);
//            }
//        }
//    }

//    protected virtual void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other != null && other.CompareTag("PlayerBullets"))
//        {
//            // Debug.Log("Enemy got hit!");
//            PlayerBullet bullet = other.GetComponent<PlayerBullet>();

//            if (bullet != null)
//            {
//                TakeDamage(bullet.Damage);

//                other.gameObject.SetActive(false);
//                spriteRenderer.color = Color.red;
//                Invoke("ResetColor", 0.5f);


//            }
//        }
//    }
//    void ResetColor()
//    {
//        spriteRenderer.color = originalColor;
//    }

//    public virtual void TakeDamage(int damage)
//    {
//        health -= damage;

//        if (health <= 0)
//        {
//            Explode();
//            gameObject.SetActive(false);
//            if (enemyWaveManager != null)
//            {
//                enemyWaveManager.EnemyDestroyed(transform.position);
//            }
//        }
//    }

//    protected void Explode()
//    {

//        PoolManager.Instance.SpawnFromPool("ShipExplosion", transform.position, Quaternion.identity);
//        DropCoin();
//    }

//    private void DropCoin()
//    {
//        if (MainMenu.mode == MainMenu.Mode.Campaign)
//        {
//            float randomValue = Random.Range(0f, 1f);
//            if (randomValue <= coinDroppingProbability)
//            {
//                PoolManager.Instance.SpawnFromPool("coins", transform.position, Quaternion.identity);
//            }
//            else if (randomValue <= powerUpDroppingProbability)
//            {
//                SpawnPowerup();
//            }

//            else 
//            {
//                PoolManager.Instance.SpawnFromPool("token", transform.position, Quaternion.identity);
//            }
//        }

//        else
//        {
//            SpawnPowerup();
//        }
//    }
//    public void SpawnPowerup()
//    {
//        if (powerupPrefabs.Count > 0)
//        {
//            int randomIndex = Random.Range(0, powerupPrefabs.Count);
//            while (randomIndex == 1 && MainMenu.mode != MainMenu.Mode.Campaign){
//                randomIndex = Random.Range(0, powerupPrefabs.Count);
//            }
//            string selectedPowerup = powerupPrefabs[randomIndex];
//            PoolManager.Instance.SpawnFromPool(selectedPowerup, transform.position, Quaternion.identity);
//        }
//    }
//    private void OnDisable() {
//        PoolManager.Instance.ReturnToPool(gameObject.tag, gameObject);
//    }
//    private void OnDestroy() {
//        Debug.Log("EnemyShip Destroyed");
//    }
//}
using UnityEngine;
using System.Collections.Generic;

public class EnemyShip : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    protected float speed; // Speed of the enemy ship
    protected float limitXPosition; // X position where the ship stops
    [SerializeField] private GameObject tokenPrefab;
    [SerializeField] private List<string> powerupPrefabs;
    protected float fireInterval; // Interval between consecutive bullet fires
    protected float bulletSpeed; // Speed of the fired bullets
    protected int health;
    protected int MAX_HEALTH;
    protected int damage;
    protected string bulletTag;

    private EnemyWaveManager enemyWaveManager;
    protected float angle;
    protected float coinDroppingProbability;
    protected float tokenDroppingProbability;
    protected float powerUpDroppingProbability;

    private PlayerControls player;
    protected float nextFireTime; // Time when the ship can fire next

    private void OnEnable()
    {
        health = MAX_HEALTH;
    }

    protected virtual void Start()
    {
        nextFireTime = Time.time;
        transform.rotation = Quaternion.Euler(0, 0, 90);

        enemyWaveManager = FindObjectOfType<EnemyWaveManager>();
        player = FindObjectOfType<PlayerControls>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("Missing SpriteRenderer component on " + gameObject.name);
            return;
        }
        originalColor = spriteRenderer.color;
    }

    protected virtual void Update()
    {
        if (transform.position.x >= limitXPosition)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else
        {
            if (Time.time > nextFireTime)
            {
                FireBullet();
                nextFireTime = Time.time + fireInterval;
            }
        }
    }

    protected virtual void FireBullet()
    {
        if (PoolManager.Instance != null)
        {
            GameObject bullet = PoolManager.Instance.SpawnFromPool(bulletTag, transform.position, Quaternion.identity);
            if (bullet != null)
            {
                PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();
                if (bulletScript != null && player != null)
                {
                    Quaternion rotation = Quaternion.Euler(0, 0, angle);
                    bulletScript.Initialize(player.transform.position, bulletSpeed, damage, rotation);
                }
                else
                {
                    Debug.LogWarning("PlayerBullet script or player is null.");
                }
            }
            else
            {
                Debug.LogWarning("Failed to spawn bullet from pool.");
            }
        }
        else
        {
            Debug.LogWarning("PoolManager instance is null.");
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("PlayerBullets"))
        {
            // Debug.Log("Enemy got hit!");
            PlayerBullet bullet = other.GetComponent<PlayerBullet>();

            if (bullet != null)
            {
                TakeDamage(bullet.Damage);

                other.gameObject.SetActive(false);
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.red;
                    Invoke("ResetColor", 0.5f);
                }
                else
                {
                    Debug.LogWarning("SpriteRenderer is null in OnTriggerEnter2D.");
                }
            }
            else
            {
                Debug.LogWarning("PlayerBullet component not found on other.");
            }
        }
    }

    void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer is null in ResetColor.");
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Explode();
            gameObject.SetActive(false);
            if (enemyWaveManager != null)
            {
                enemyWaveManager.EnemyDestroyed(transform.position);
            }
            else
            {
                Debug.LogWarning("EnemyWaveManager is null in TakeDamage.");
            }
        }
    }

    protected void Explode()
    {
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.SpawnFromPool("ShipExplosion", transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("PoolManager instance is null in Explode.");
        }
        DropCoin();
    }

    private void DropCoin()
    {
        if (MainMenu.mode == MainMenu.Mode.Campaign)
        {
            float randomValue = Random.Range(0f, 1f);
            if (randomValue <= coinDroppingProbability)
            {
                PoolManager.Instance.SpawnFromPool("coins", transform.position, Quaternion.identity);
            }
            else if (randomValue <= powerUpDroppingProbability)
            {
                SpawnPowerup();
            }
            else
            {
                PoolManager.Instance.SpawnFromPool("token", transform.position, Quaternion.identity);
            }
        }
        else
        {
            SpawnPowerup();
        }
    }

    public void SpawnPowerup()
    {
        if (powerupPrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, powerupPrefabs.Count);
            while (randomIndex == 1 && MainMenu.mode != MainMenu.Mode.Campaign)
            {
                randomIndex = Random.Range(0, powerupPrefabs.Count);
            }
            string selectedPowerup = powerupPrefabs[randomIndex];
            if (PoolManager.Instance != null)
            {
                PoolManager.Instance.SpawnFromPool(selectedPowerup, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("PoolManager instance is null in SpawnPowerup.");
            }
        }
        else
        {
            Debug.LogWarning("Powerup prefabs list is empty or null.");
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
        Debug.Log("EnemyShip Destroyed");
    }
}
