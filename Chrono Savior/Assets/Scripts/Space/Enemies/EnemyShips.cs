using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    protected float speed; // Speed of the enemy ship
    protected float limitXPosition; // X position where the ship stops
    public GameObject bulletPrefab; // Prefab of the bullet
    public GameObject explosion; // Prefab of the explosion
    public GameObject coinPrefab; // Prefab of the coins
    public GameObject tokenPrefab;
    public float fireInterval; // Interval between consecutive bullet fires
    public float bulletSpeed; // Speed of the fired bullets
    protected int health = 10;
    protected int damage;
    private EnemyWaveManager enemyWaveManager;
    protected float angle;
    protected float coinDroppingProbability;
    private PlayerControls player;
    protected float nextFireTime; // Time when the ship can fire next

    protected virtual void Start()
    {
        // Initialize next fire time
        nextFireTime = Time.time;

        // Rotate the enemy ship by 90 degrees
        transform.rotation = Quaternion.Euler(0, 0, 90);

        enemyWaveManager = FindObjectOfType<EnemyWaveManager>();
        player = FindObjectOfType<PlayerControls>();
<<<<<<< Updated upstream
        
=======
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

>>>>>>> Stashed changes

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
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();
            if (bulletScript != null && player != null)
            {
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                bulletScript.Initialize(player.transform.position, bulletSpeed, damage, rotation);
            }
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
<<<<<<< Updated upstream
                Destroy(other.gameObject);
=======
                other.gameObject.SetActive(false);
                spriteRenderer.color = Color.red;
                Invoke("ResetColor", 0.5f);

>>>>>>> Stashed changes
            }
        }
    }
    void ResetColor()
    {
        spriteRenderer.color = originalColor;
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Explode();
            Destroy(gameObject);
            if (enemyWaveManager != null)
            {
                enemyWaveManager.EnemyDestroyed();
            }
        }
    }

    protected void Explode()
    {
        if (explosion != null)
        {
            GameObject explosionObject = Instantiate(explosion, transform.position, Quaternion.identity);
        }
        DropCoin();
    }

    private void DropCoin()
    {
        if (coinPrefab != null && tokenPrefab != null && MainMenu.mode == MainMenu.Mode.Campaign)
        {
            float randomValue = Random.Range(0f, 1f);
            if (randomValue <= coinDroppingProbability)
            {
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(tokenPrefab, transform.position, Quaternion.identity);
            }
        }
<<<<<<< Updated upstream
=======
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
            while (randomIndex == 1 && MainMenu.mode != MainMenu.Mode.Campaign){
                randomIndex = Random.Range(0, powerupPrefabs.Count);
            }
            GameObject selectedPowerup = powerupPrefabs[randomIndex];
            Instantiate(selectedPowerup,transform.position, Quaternion.identity);
        }
>>>>>>> Stashed changes
    }
}
