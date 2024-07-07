using UnityEngine;
using System.Collections.Generic;
using Pathfinding; // Assuming Seeker and Path are from the A* Pathfinding Project

public abstract class GroundEnemyScript : MonoBehaviour
{
    [SerializeField] protected List<OnPowerupInteract> powerUps = new List<OnPowerupInteract>();
    [SerializeField] protected float MAX_HEALTH = 70f;
    [SerializeField] protected Animator animator;
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Seeker seeker;
    protected float currentHealth;
    protected GameObject playerCenter;
    protected Player player;
    protected float nextWaypointDistance = 0.5f;
    protected bool isInfinite = false;
    protected Path path;
    protected int currentWayPoint = 0;
    protected float timer = 0;
    protected const string UPDATE_PATH = "UpdatePath";
    protected const string PLAYER_CENTER = "PlayerCenter";

    protected virtual void Start()
    {
        currentHealth = MAX_HEALTH;
        player = Player.Instance;
        playerCenter = GameObject.FindGameObjectWithTag(PLAYER_CENTER);

        if (seeker == null)
        {
            Debug.LogError("Seeker component is not assigned in " + GetType().Name);
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

    protected virtual void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
    protected virtual void UpdatePath()
    {
        
        if (seeker == null)
        {
            Debug.LogError("Seeker component is null");
            return;
        }
        if (playerCenter == null)
        {
            Debug.LogError("PlayerCenter GameObject is null");
            return;
        }
        if (seeker.IsDone() && playerCenter != null)
        {
            seeker.StartPath(rb.position, playerCenter.transform.position, OnPathComplete);
        }
    }

    protected abstract void Update();
    protected abstract void FixedUpdate();

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }


    protected abstract void Die();

    

    

    protected virtual void SpawnPowerUp()
    {
        if (powerUps.Count == 0)
        {
            Debug.LogWarning("No power-ups assigned in " + GetType().Name);
            return;
        }

        int index = Random.Range(0, powerUps.Count);
        if (powerUps[index] == null)
        {
            Debug.LogError("PowerUp at index " + index + " is null in " + GetType().Name);
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
            Debug.LogError("PowerupPoolingAPI is null in " + GetType().Name);
        }
    }

    protected virtual void SpawnCoin()
    {
        if (CoinPoolingAPI.SharedInstance != null)
        {
            CoinScript coin = CoinPoolingAPI.SharedInstance.GetPooledCoin();
            coin.transform.position = transform.position;
            coin.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("CoinPoolingAPI is null in " + GetType().Name);
        }
    }
}
