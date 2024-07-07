using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GunRobot : MonoBehaviour, IEnemy
{
    [SerializeField] private List<OnPowerupInteract> powerUps = new List<OnPowerupInteract>();
    [SerializeField] private float MAX_HEALTH = 70f;
    [SerializeField] private Animator animator;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Seeker seeker;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPosition;
    private float currentHealth;
    private GameObject playerCenter;
    private Player player;
    private float nextWaypointDistance = 0.5f;
    private bool isInfinite = false;
    private Path path;
    private int currentWayPoint = 0;
    private float timer = 0;
    private const float TIME_BETWEEN_SHOTS = 0.5f;
    private const float SHOOT_DISTANCE = 5f;
    private const float AWAY_DISTANCE = 2f;
    private bool inRange = false;
    private const string UPDATE_PATH = "UpdatePath";
    private const string PLAYER_CENTER = "PlayerCenter";
    private const string IS_SHOOTING = "IsShooting";

    void Start()
    {
        currentHealth = MAX_HEALTH;
        player = Player.Instance;
        playerCenter = GameObject.FindGameObjectWithTag(PLAYER_CENTER);

        if (seeker == null)
        {
            Debug.LogError("Seeker component is not assigned in GunRobot.");
        }
        if (playerCenter == null)
        {
            Debug.LogError("PlayerCenter GameObject is not found.");
        }
        if (seeker != null && playerCenter != null)
        {
            InvokeRepeating(UPDATE_PATH, 0f, 0.5f);
            seeker.StartPath(rb.position, playerCenter.transform.position, OnPathComplete);
        }

        if (MainMenu.mode == MainMenu.Mode.Infinity)
        {
            isInfinite = true;
        }
    }

    void UpdatePath()
    {
        if (seeker == null)
        {
            Debug.LogError("Seeker component is null in GunRobot.");
            return;
        }
        if (seeker.IsDone() && playerCenter != null)
        {
            seeker.StartPath(rb.position, playerCenter.transform.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void Update()
    {
        if (player == null || playerCenter == null || rb == null)
        {
            Debug.LogError("Player, PlayerCenter or Rigidbody2D is null in GunRobot.");
            return;
        }

        if (player.AreEnemiesFrozen())
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 distanceVec = playerCenter.transform.position - transform.position;
        float distanceSquared = distanceVec.sqrMagnitude;
        if (distanceSquared <= (SHOOT_DISTANCE * SHOOT_DISTANCE))
        {
            inRange = distanceSquared <= (AWAY_DISTANCE * AWAY_DISTANCE);

            rb.velocity = Vector3.zero;
            timer += Time.deltaTime;
            if (timer > TIME_BETWEEN_SHOTS)
            {
                Shoot();
                timer = 0;
            }
        }
        else
        {
            inRange = false;
        }
    }

    void FixedUpdate()
    {
        if (player == null || rb == null)
        {
            Debug.LogError("Player or Rigidbody2D is null in GunRobot.");
            return;
        }
        if (path == null || inRange || player.AreEnemiesFrozen()) return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        transform.localScale = new Vector3(direction.x >= 0.01f ? 1f : -1f, 1f, 1f);
        rb.MovePosition(rb.position + (direction * movementSpeed * Time.fixedDeltaTime));
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Shoot()
    {
        Instantiate(bullet, bulletPosition.position, Quaternion.identity);
        if (animator != null)
        {
            animator.SetTrigger(IS_SHOOTING);
        }
        else
        {
            Debug.LogError("Animator component is null in GunRobot.");
        }
    }

    private void SpawnPowerUp()
    {
        if (powerUps.Count == 0)
        {
            Debug.LogWarning("No power-ups assigned in GunRobot.");
            return;
        }

        int index = Random.Range(0, powerUps.Count);
        if (powerUps[index] == null)
        {
            Debug.LogError("PowerUp at index " + index + " is null in GunRobot.");
            return;
        }

        if (PowerupPoolingAPI.SharedInstance != null)
        {
            OnPowerupInteract powerup = PowerupPoolingAPI.SharedInstance.GetPooledPowerup(index);
            powerup.transform.position = transform.position;
            powerup.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("PowerupPoolingAPI is null in GunRobot.");
        }
    }

    private void SpawnCoin()
    {
        if (CoinPoolingAPI.SharedInstance != null)
        {
            CoinScript coin = CoinPoolingAPI.SharedInstance.GetPooledCoin();
            coin.transform.position = transform.position;
            coin.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("CoinPoolingAPI is null in GunRobot.");
        }
    }

    private void Die()
    {
        Destroy(gameObject);

        int randomNumber = Random.Range(0, 5);
        if (randomNumber == 0)
        {
            SpawnPowerUp();
        }
        else if (randomNumber == 1 && !isInfinite)
        {
            SpawnCoin();
        }

        if (!isInfinite && StoryManager.Instance != null)
        {
            StoryManager.Instance.DecreaseEnemyCount();
        }
        else if (isInfinite && StateManagement.Instance != null)
        {
            StateManagement.Instance.SetGroundKillCount(StateManagement.Instance.GetGroundKillCount() + 1);
        }
    }
}
