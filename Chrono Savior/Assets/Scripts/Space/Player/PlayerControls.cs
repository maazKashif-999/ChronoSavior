using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour
{
    public float speed = 5f; // Speed of the player's movement
    float minY; // Minimum Y position
    float maxY; // Maximum Y position
    public GameObject explosion;
    public TextMeshProUGUI coinCount; // Reference to the TextMeshPro text element for displaying coin count
    public TextMeshProUGUI tokenCount;
    float health;
    float shield;
    public HealthBar healthBar;

    public ShieldBar shieldBar;
    int maxHealth = 100;
    int maxShield = 50;
    float bulletSpeed = 6.0f;
    int damage = 10;
    public GameObject playerBulletPrefab; // Prefab of the player bullet
    private SpaceGameManager gameManager; // Reference to the GameManager
    private int coins = 0; // Variable to keep track of collected coins
    private int token = 0;

    public AudioClip bulletSound; //variable to store bullet sound
    public AudioClip explodeSound;
    private AudioSource audioSource;



    private void Start()
    {
        gameManager = FindObjectOfType<SpaceGameManager>(); // Find the GameManager in the scene
        audioSource = GetComponent<AudioSource>();

    }

    public void Init()
    {
        gameObject.SetActive(true);
        health = maxHealth; // Reset health to max
        shield = maxShield;
        coins = 0; // Reset coin count
        token = 0; 
        UpdateHealth();
        UpdateShield();

        if (tokenCount != null)
        {
            tokenCount.text = token.ToString();
        }

        if (coinCount != null)
        {
            coinCount.text = coins.ToString(); // Update coin count UI
        }

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float playerHeight = GetComponent<SpriteRenderer>().bounds.size.y;
            minY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + playerHeight / 2;
            maxY = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).y - playerHeight / 2;
        }
        else
        {
            Debug.LogWarning("Main camera is not assigned.");
        }

        transform.position = new Vector3(-6, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, 270);
    }

    void Update()
    {
        // Fire bullet when user clicks left mouse button or touches screen
        if (Input.GetMouseButtonDown(0))
        {

            FireBullet();
        }
        Move();
    }

    void Move()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 newPosition = transform.position + new Vector3(0, verticalInput * speed * Time.deltaTime, 0);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = newPosition;
    }

    void FireBullet()
    {
        if (playerBulletPrefab != null)
        {
            GameObject bulletObject = Instantiate(playerBulletPrefab, transform.position, Quaternion.identity);
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                PlayerBullet bulletScript = bulletObject.GetComponent<PlayerBullet>();
                if (bulletScript != null)
                {
                    bulletScript.Initialize(mousePosition, bulletSpeed, damage, Quaternion.Euler(0, 0, 0));
                }

                if (bulletSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(bulletSound);
                }
            }
            else
            {
                Debug.LogWarning("Main camera is not assigned.");
            }
        }
        else
        {
            Debug.LogWarning("Player bullet prefab is not assigned.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
            return;

        if (other.CompareTag("EnemyBullets"))
        {
            PlayerBullet bullet = other.GetComponent<PlayerBullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("coins"))
        {
            coins++;
            Debug.Log(coins);

            if (coinCount != null)
            {
                coinCount.text = coins.ToString();
            }
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("token"))
        {
            token += 20;

            if (tokenCount != null)
            {
                tokenCount.text = token.ToString();
            }
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (shield > 0)
        {
            shield -= damage;
            Mathf.Clamp(shield, 0, maxShield);
            UpdateShield();
        }
        else{
            health -= damage;
            health = Mathf.Clamp(health, 0, maxHealth);
            UpdateHealth();
        }
        
        if (health <= 0)
        {
            Explode();
            StartCoroutine(WaitAndEndGame(0.4f));
        }
    }

    void Explode()
    {
        if (explosion != null)
        {
            GameObject explosionObject = Instantiate(explosion, transform.position, Quaternion.identity);
        }

        if (explodeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explodeSound);
        }

        else
        {
            Debug.LogWarning("Explosion prefab is not assigned.");
        }
    }

    void UpdateHealth()
    {
        float healthAmount = health / maxHealth;
        if (healthBar != null)
        {
            healthBar.SetHealth(healthAmount);
        }
        else
        {
            Debug.LogWarning("Health bar is not assigned.");
        }
    }
    void UpdateShield()
    {
        float healthAmount = shield / maxShield;
        if (shieldBar != null)
        {
            shieldBar.SetSheild(healthAmount);
        }
        else
        {
            Debug.LogWarning("Shield bar is not assigned.");
        }
    }


    IEnumerator WaitAndEndGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (gameManager != null)
        {
            gameManager.EndGame();
        }
        else
        {
            Debug.LogWarning("Game manager is not assigned.");
        }
    }
}
