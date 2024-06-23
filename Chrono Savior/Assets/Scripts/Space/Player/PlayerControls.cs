using UnityEngine;
using TMPro;
using System.Collections; // Required for IEnumerator
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
    public HealthBar healthBar;
    int maxHealth = 100;
    float bulletSpeed = 6.0f;

    int damage = 10;
    public GameObject playerBulletPrefab; // Prefab of the player bullet
    private GameManager gameManager; // Reference to the GameManager

    private int coins = 0; // Variable to keep track of collected coins
    private int token = 0;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
    }

    public void Init()
    {
        gameObject.SetActive(true);
        health = maxHealth; // Reset health to max
        coins = 0; // Reset coin count
        token = 0; 
        UpdateHealth();
        tokenCount.text = token.ToString();
        coinCount.text = coins.ToString(); // Update coin count UI
        minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        maxY = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)).y;
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
        GameObject bulletObject = Instantiate(playerBulletPrefab, transform.position, Quaternion.identity);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; 
        PlayerBullet bulletScript = bulletObject.GetComponent<PlayerBullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(mousePosition, bulletSpeed, damage, Quaternion.Euler(0, 0, 0));
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
            coinCount.text = coins.ToString();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("token"))
        {
            token += 20;
            tokenCount.text = token.ToString();
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealth();

        if (health <= 0)
        {
            Explode();

            StartCoroutine(WaitAndEndGame(0.4f));
        }
    }

    void Explode()
    {
        GameObject explosionObject = Instantiate(explosion, transform.position, Quaternion.identity);
    }

    void UpdateHealth()
    {
        float healthAmount = health / maxHealth;
        healthBar.SetHealth(healthAmount);
    }

    IEnumerator WaitAndEndGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameManager.endGame();
    }
}
