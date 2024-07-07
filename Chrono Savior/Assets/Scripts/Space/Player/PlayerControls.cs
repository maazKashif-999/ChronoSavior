
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    public static PlayerControls Instance { get; private set; }

    float speed = 5f;
    float minY; // Minimum Y position
    float maxY; // Maximum Y position

    [SerializeField] private GameObject explosion;
    [SerializeField] private Text coinCount; // Reference to the TextMeshPro text element for displaying coin count
    [SerializeField] private Text tokenCount;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    float health;
    float shield;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private AudioClip bulletSound; //bullet picking up sound clip
    [SerializeField] private AudioClip tokenPickupSound; //token picking up sound clip
    [SerializeField] private AudioClip coinPickupSound;
    [SerializeField] private AudioClip powerupSound;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip damageSoundSpaceship;
    private AudioSource audioSource; // AudioSource component

    [SerializeField] private ShieldBar shieldBar;
    const int MAX_HEALTH = 100;
    const int MAX_SHIELD = 50;
    float bulletSpeed = 6.0f;
    int damage;
    [SerializeField] private GameObject playerBulletPrefab; // Prefab of the player bullet
    private SpaceGameManager gameManager; // Reference to the GameManager
    private int coins = 0; // Variable to keep track of collected coins
    private int token = 0;

    private float fireRate, fireTimer;
    const int MULTIPLIER = 20;

    int multi;

    const int MAX_DAMAGE = 10;

    public SpriteRenderer Render
    {
        get { return spriteRenderer; }
    }
    public int Multi
    {
        get { return multi; }
        set { multi = value; }
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
        set
        {
            health = MAX_HEALTH;
            UpdateHealth();
        }
    }
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public int Shield
    {
        set
        {
            shield = MAX_SHIELD;
            UpdateShield();
        }
    }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of Player found");
        }
        Instance = this;
    }

    private void Start()
    {
        gameManager = FindObjectOfType<SpaceGameManager>(); // Find the GameManager in the scene
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer is not assigned.");
        }
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
        fireRate = 0.2f;
        fireTimer = 0f;
        UpdateHealth();
        UpdateShield();

        if (tokenCount != null)
        {
            tokenCount.text = token.ToString();
        }
        else
        {
            Debug.LogWarning("TokenCount is not assigned.");
        }

        if (coinCount != null)
        {
            coinCount.text = coins.ToString(); // Update coin count UI
        }
        else
        {
            Debug.LogWarning("CoinCount is not assigned.");
        }

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float playerHeight = GetComponent<SpriteRenderer>().bounds.size.y;
            float playerHeightX = GetComponent<SpriteRenderer>().bounds.size.x + 0.1f;
            minY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + playerHeight / 2;
            maxY = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).y - playerHeight / 2;
            transform.position = new Vector3(mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + playerHeightX, 0, 0);
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
        if (Input.GetMouseButton(0))
        {
            fireTimer += Time.deltaTime;

            if (fireTimer >= fireRate)
            {
                FireBullet();
                fireTimer = 0f; // Reset the timer
            }
        }
        else
        {
            // Reset the timer when the mouse button is released to avoid immediate firing when pressed again
            fireTimer = fireRate;
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
        if (direction.x < 0) return;
        // Calculate angle to rotate towards mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the object towards the calculated angle (assuming the object is 2D)
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 270));
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
        if (PauseMenu.gameIsPaused) return;

        if (playerBulletPrefab != null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (mousePosition.x < transform.position.x)
                {
                    return;
                }
                mousePosition.z = 0f;
                GameObject bulletObject = PoolManager.Instance.SpawnFromPool("PlayerBullets", transform.position, Quaternion.identity);
                if (bulletObject != null)
                {
                    PlayerBullet bulletScript = bulletObject.GetComponent<PlayerBullet>();
                    if (bulletScript != null)
                    {
                        bulletScript.Initialize(mousePosition, bulletSpeed, damage, Quaternion.Euler(0, 0, 0));
                    }
                    else
                    {
                        Debug.LogWarning("PlayerBullet component not found on bulletObject.");
                    }

                    if (audioSource != null && bulletSound != null)
                    {
                        audioSource.PlayOneShot(bulletSound);
                    }
                }
                else
                {
                    Debug.LogWarning("Failed to spawn bullet from pool.");
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

        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullets"))
        {
            PlayerBullet bullet = other.GetComponent<PlayerBullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.Damage);
                other.gameObject.SetActive(false);
            }
        }
    }

    public void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer is not assigned.");
        }
    }

    public void TakeDamage(int damage)
    {
        if (shield > 0)
        {

            shield -= damage;
            Mathf.Clamp(shield, 0, MAX_SHIELD);
            UpdateShield();
        }
        else
        {
            health -= damage;
            health = Mathf.Clamp(health, 0, MAX_HEALTH);
            UpdateHealth();
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }

        else
        {
            Debug.LogWarning("SpriteRenderer is not assigned.");
        }
        playDamageSound();

        Invoke("ResetColor", 0.5f);

        if (health <= 0)
        {
            Explode();
            StartCoroutine(WaitAndEndGame(0.1f));
        }
    }

    void Explode()
    {
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.SpawnFromPool("ShipExplosion", transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("PoolManager instance is null in Explode.");
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
            shieldBar.SetShield(healthAmount);
        }
        else
        {
            Debug.LogWarning("Shield bar is not assigned.");
        }
    }

    public void UpdateToken()
    {
        token += multi;

        if (tokenCount != null)
        {
            tokenCount.text = token.ToString();
        }
        else
        {
            Debug.LogWarning("TokenCount is not assigned.");
        }

        if (audioSource != null && tokenPickupSound != null)
        {
            audioSource.PlayOneShot(tokenPickupSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or TokenPickupSound is not assigned.");
        }
    }

    public void UpdateCoin()
    {
        coins++;
        if (coinCount != null)
        {
            coinCount.text = coins.ToString();
        }
        else
        {
            Debug.LogWarning("CoinCount is not assigned.");
        }

        if (audioSource != null && coinPickupSound != null)
        {
            audioSource.PlayOneShot(coinPickupSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or CoinPickupSound is not assigned.");
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

    public float GetCurrentHealth()
    {
        return health;
    }

    public float GetCurrentShield()
    {
        return shield;
    }

    public int GetCoins()
    {
        return coins;
    }

    public int GetTokens()
    {
        return token;
    }

    public void PlayPowerupSound()
    {
        if (audioSource != null && powerupSound != null)
        {
            audioSource.PlayOneShot(powerupSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or PowerupSound is not assigned.");
        }
    }

    public void PlayExplosionSound()
    {
        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or ExplosionSound is not assigned.");
        }
    }


    public void playDamageSound()
    {
        if (audioSource != null && damageSoundSpaceship != null)
        {
            audioSource.PlayOneShot(damageSoundSpaceship);
            //Debug.Log("Damage sound for ship is found");

        }
        else
        {
            Debug.Log("damage sound not found");
        }
    }
}
