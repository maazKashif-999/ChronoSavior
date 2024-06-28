using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour
{

    float speed = 5f; 
    float minY; // Minimum Y position
    float maxY; // Maximum Y position
    public GameObject explosion;
    public TextMeshProUGUI coinCount; // Reference to the TextMeshPro text element for displaying coin count
    public TextMeshProUGUI tokenCount;
    float health;
    float shield;
    public HealthBar healthBar;

    public ShieldBar shieldBar;
    const int MAX_HEALTH = 100;
    const int MAX_SHIELD = 50;
    float bulletSpeed = 6.0f;
    int damage ;
    public GameObject playerBulletPrefab; // Prefab of the player bullet
    private SpaceGameManager gameManager; // Reference to the GameManager
    private int coins = 0; // Variable to keep track of collected coins
    private int token = 0;

    const int MULTIPLIER = 20;

    int multi;

    const int MAX_DAMAGE = 10;

    public int Multi
    {
        get { return multi; }
        set {multi = value;}
    }
    public int Multiplier
    {
        get { return MULTIPLIER; }
    }
    public int MaxDamage
    {
        get { return MAX_DAMAGE; }
    }

    public int Health
    {
        set { health = MAX_HEALTH; }
    }
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public int Shield
    {
        set { shield = MAX_SHIELD; }
    }
 
    private void Start()
    {
        gameManager = FindObjectOfType<SpaceGameManager>(); // Find the GameManager in the scene
    }

    public void Init()
    {
        gameObject.SetActive(true);
        health = MAX_HEALTH; // Reset health to max
        shield = MAX_SHIELD;
        damage = MAX_DAMAGE;
        multi = MULTIPLIER;
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
        
            
            transform.position = new Vector3(mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + 0.3f, 0, 0);
        }
        else
        {
            Debug.LogWarning("Main camera is not assigned.");
        }

        
        transform.rotation = Quaternion.Euler(0, 0, 270);
    }

    void Update()
    {
        // Fire bullet when user clicks left mouse button or touches screen
        if (Input.GetMouseButtonDown(0))
        {
            FireBullet();
        }
        RotateTowardsMouse();
        
        Move();
    }

    void RotateTowardsMouse()
        {
            // Get the position of the mouse in screen space
            Vector3 mousePosition = Input.mousePosition;

            // Convert mouse position to world space
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Calculate direction towards mouse
            Vector2 direction = (mousePosition - transform.position).normalized;

            // Calculate angle to rotate towards mouse
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate the object towards the calculated angle (assuming the object is 2D)
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle+270));
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
                TakeDamage(bullet.Damage);
                Destroy(other.gameObject);
            }
        }
        // else if (other.CompareTag("coins"))
        // {
        //     coins++;
        //     Debug.Log(coins);

        //     if (coinCount != null)
        //     {
        //         coinCount.text = coins.ToString();
        //     }
        //     Destroy(other.gameObject);
        // }
        // else if (other.CompareTag("token"))
        // {
        //     token += multi;

        //     if (tokenCount != null)
        //     {
        //         tokenCount.text = token.ToString();
        //     }
        //     Destroy(other.gameObject);
        // }
    }
    public void TakeDamage(int damage)
    {
        if (shield > 0)
        {
            shield -= damage;
            Mathf.Clamp(shield, 0, MAX_SHIELD);
            UpdateShield();
        }
        else{
            health -= damage;
            health = Mathf.Clamp(health, 0, MAX_HEALTH);
            UpdateHealth();
        }
        
        if (health <= 0)
        {
            Explode();
            StartCoroutine(WaitAndEndGame(0.1f));
        }
    }
    void Explode()
    {
        if (explosion != null)
        {
            GameObject explosionObject = Instantiate(explosion, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Explosion prefab is not assigned.");
        }
    }
    void UpdateHealth()
    {
        float healthAmount = health / MAX_HEALTH;
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
        float healthAmount = shield / MAX_SHIELD;
        if (shieldBar != null)
        {
            shieldBar.SetSheild(healthAmount);
        }
        else
        {
            Debug.LogWarning("Shield bar is not assigned.");
        }
    }

    public void UpdateToekn()
    {
        token += multi;

        if (tokenCount != null)
        {
            tokenCount.text = token.ToString();
        }
    }

    public void UpdateCoin()
    {
        coins++;
        if (coinCount != null)
        {
            coinCount.text = coins.ToString();
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
