using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public delegate void CoinGet();
    public event CoinGet OnCoinGet;
    private bool isAlive = true;
    private const float MAX_HEALTH = 100f;
    public const float MAX_SHIELD = 50f;
    private float currentHealth = 100f;
    private float currentShield = 50f;
    private float horizontalInput;
    private float verticalInput;
    private const float MOVE_SPEED = 5f;
    private float speedMultipler = 1f;
    private float damageMultipler = 1f;
    private bool isWalking;
    private bool canUseAbility = true;
    private bool areEnemiesFrozen = false;
    private bool redSkinEquipped = true;
    private int coins = 0;
    private bool isInfinite = false;
    private Camera mainCamera;
    [SerializeField] GameObject playerBlue;
    [SerializeField] GameObject playerRed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private GameObject playerDeath;
    private GameOverController gameOverController;
    private GameTimer gameTimer;

    void Awake()
    {
        mainCamera = Camera.main;
        gameOverController = FindObjectOfType<GameOverController>();
        gameTimer = FindObjectOfType<GameTimer>();

        if (Instance != null)
        {
            Debug.LogError("Multiple instances of Player found");
        }
        Instance = this;
        if (gameOverController == null)
        {
            Debug.LogError("GameOverController not found in the scene.");
        }

        
    }

    void Start()
    {
        if(StateManagement.Instance != null)
        {
            redSkinEquipped = StateManagement.Instance.IsRedSkinEquipped();
            if (playerBlue != null && playerRed != null)
            {
                if (redSkinEquipped)
                {
                    playerBlue.SetActive(false);
                    playerRed.SetActive(true);
                }
                else
                {
                    playerBlue.SetActive(true);
                    playerRed.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("PlayerBlue or PlayerRed not found in Player.");
            }
        }
        if(MainMenu.mode == MainMenu.Mode.Infinity)
        {
            isInfinite = true;
        }
        else
        {
            isInfinite = false;
        }
    }

    void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            return;
        }
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        isWalking = horizontalInput != 0 || verticalInput != 0;
        Flip();

        if (Input.GetKeyDown(KeyCode.Z) && canUseAbility)
        {
            StartCoroutine(FreezeEnemiesCo());
        }
    }

    void FixedUpdate()
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found in Player.");
            return;
        }
        if (!isAlive)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        Vector2 moveDir = new Vector2(horizontalInput, verticalInput).normalized;
        rb.MovePosition(rb.position + (moveDir * MOVE_SPEED * speedMultipler * Time.fixedDeltaTime));
    }

    private void Flip()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePosition - transform.position;
        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        if (playerBlue != null && playerRed != null && mainCamera != null)
        {
            if (rotationZ > -90 && rotationZ < 90) // || horizontalInput > 0 
            {
                playerBlue.transform.rotation = Quaternion.Euler(0, 0, 0);
                playerRed.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if ((rotationZ >= 90 && rotationZ <= 180) || (rotationZ <= -90 && rotationZ >= -180)) // (horizontalInput < 0)
            {
                playerBlue.transform.rotation = Quaternion.Euler(0, -180, 0);
                playerRed.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
        }
        else
        {
            Debug.LogError("PlayerBlue, MainCamera or PlayerRed not found in Player.");
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public void TakeDamage(float damage)
    {
        if (currentShield > 0)
        {
            currentShield -= damage;
            if (currentShield < 0)
            {
                currentShield = 0;
            }
        }
        else
        {
            currentHealth -= damage;
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Instantiate(playerDeath, new Vector2(transform.position.x, transform.position.y - 0.25f), Quaternion.identity);
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        isAlive = false;
        yield return new WaitForSeconds(0.6f);
        if(StateManagement.Instance != null)
        {
            int total_coins = StateManagement.Instance.GetCoins();
            total_coins += coins;
            StateManagement.Instance.SetCoins(total_coins);
        }
        if (gameOverController != null)
        {
            gameOverController.ShowGameOverScreen();
        }
        if(isInfinite && gameTimer != null) 
        {
            gameTimer.StopTimer();
        }
        areEnemiesFrozen = true;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void FullHealthHeal()
    {
        currentHealth = MAX_HEALTH;
    }

    public void FullShieldHeal()
    {
        currentShield = MAX_SHIELD;
    }

    public void SpeedBoost()
    {
        speedMultipler = 1.5f;
        StartCoroutine(ResetBoostCo());
    }

    public void ResetSpeed()
    {
        speedMultipler = 1f;
    }

    IEnumerator ResetBoostCo()
    {
        yield return new WaitForSeconds(10f);
        ResetSpeed();
    }

    public void DamageBoost()
    {
        damageMultipler = 2f;
        StartCoroutine(ResetDamageCo());
    }

    public void ResetDamage()
    {
        damageMultipler = 1f;
    }

    IEnumerator ResetDamageCo()
    {
        yield return new WaitForSeconds(10f);
        ResetDamage();
    }

    public float GetDamageMultipler()
    {
        return damageMultipler;
    }

    public void OneShotOneKill()
    {
        damageMultipler = 100f;
        StartCoroutine(ResetDamageCo());
    }

    IEnumerator FreezeEnemiesCo()
    {
        areEnemiesFrozen = true;
        canUseAbility = false;
        yield return new WaitForSeconds(10f);
        areEnemiesFrozen = false;
        yield return new WaitForSeconds(20f);
        canUseAbility = true;
    }

    public bool AreEnemiesFrozen()
    {
        return areEnemiesFrozen;
    }

    public void RemoveCooldown()
    {
        canUseAbility = true;
    }

    public bool CanUseAbility()
    {
        return canUseAbility;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetCurrentShield()
    {
        return currentShield;
    }

    public void AddCoin()
    {
        coins++;
        OnCoinGet?.Invoke();
    }

    public int GetCoins()
    {
        return coins;
    }
}
