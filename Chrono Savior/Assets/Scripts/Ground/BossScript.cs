using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossScript : MonoBehaviour, IEnemy
{
    // public delegate void SpawnEnemies();
    // public event SpawnEnemies OnSpawnEnemies;
    [SerializeField] private float MAX_HEALTH = 750f;
    [SerializeField] private Animator animator;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Seeker seeker;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject missile;
    [SerializeField] private Transform bulletPosition1;
    [SerializeField] private Transform bulletPosition2;
    [SerializeField] private SpriteRenderer invincibleSkin;
    [SerializeField] private SpriteRenderer defaultSkin;
    [SerializeField] private GameObject explosion;
    private float currentHealth;
    private GameObject playerCenter;
    private bool alreadySpawned = false;
    private Player player;
    private float nextWaypointDistance = 0.5f;
    private Path path;
    private int currentWayPoint = 0;
    private float timer = 0;
    private int coinsDropped = 15;
    private bool canTakeDamage = true;
    private const float TIME_BETWEEN_SHOTS = 0.5f;
    private const float TIME_BETWEEN_MISSILES = 2f;
    private const float SHOOT_DISTANCE = 8f;
    private const float AWAY_DISTANCE = 4f;
    private bool inRange = false;
    private const string UPDATE_PATH = "UpdatePath";
    private const string PLAYER_CENTER = "PlayerCenter";
    private const string IS_WALKING = "IsWalking";
    private const string DEATH = "Death";
    private bool firingBullets = true;

    void Start()
    {
        currentHealth = MAX_HEALTH;
        player = Player.Instance;
        playerCenter = GameObject.FindGameObjectWithTag(PLAYER_CENTER);
        if(invincibleSkin != null)
        {
            invincibleSkin.gameObject.SetActive(false);
        }
        if(defaultSkin != null)
        {
            defaultSkin.gameObject.SetActive(true);
        }
        if(seeker != null && playerCenter != null)
        {
            InvokeRepeating(UPDATE_PATH,0f,0.5f);
            seeker.StartPath(rb.position, playerCenter.transform.position, OnPathComplete);
        }
        else
        {
            Debug.LogError("Seeker or PlayerCenter is null in BossScript");
        }

    }

    void UpdatePath()
    {
        if(seeker == null)
        {
            Debug.Log("Seeker is null in BossScript");
            return;
        }
        if(seeker.IsDone())
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
        if(player == null || playerCenter == null || rb == null)
        {
            Debug.LogError("Player, PlayerCenter or Rigidbody2D is null in BossScript");
            return;
        } 
        if(!player.IsAlive()) return;
        if(!canTakeDamage) return;
        Vector2 distanceVec = playerCenter.transform.position - transform.position;
        float distanceSquared = distanceVec.sqrMagnitude;
        if(distanceSquared <= (SHOOT_DISTANCE * SHOOT_DISTANCE))
        {
            if(distanceSquared <= (AWAY_DISTANCE * AWAY_DISTANCE)) 
            {
                inRange = true;
                if(animator == null)
                {
                    Debug.LogError("Animator is null in BossScript");
                    return;
                }
                animator.SetBool(IS_WALKING,false);
            }
            else
            {
                inRange = false;
                if(animator == null)
                {
                    Debug.LogError("Animator is null in BossScript");
                    return;
                }
                animator.SetBool(IS_WALKING,true);
            }
            
            rb.velocity = new Vector3(0,0,0);
            timer += Time.deltaTime;
            if(!firingBullets)
            {
                if(timer > TIME_BETWEEN_MISSILES)
                {
                    Shoot();
                    timer = 0;
                }
            }
            else
            {
                if(timer > TIME_BETWEEN_SHOTS)
                {
                    Shoot();
                    timer = 0;
                }
            }
        }
        else
        {
            inRange = false;
            if(animator == null)
            {
                Debug.LogError("Animator is null in BossScript");
                return;
            }
            animator.SetBool(IS_WALKING,true);
        }
        

    }
    void FixedUpdate()
    {
        if(player == null || rb == null)
        {
            Debug.LogError("Player or Rigidbody2D is null in BossScript");
            return;
        }
        if(path == null || inRange || !player.IsAlive()) return;
        
        if(currentWayPoint >= path.vectorPath.Count)
        {
            return;
        }
        
        if(!canTakeDamage) return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        if(direction.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f,1f,1f);
        }
        else if(direction.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f,1f,1f);
        }
        rb.MovePosition(rb.position + (direction * movementSpeed * Time.fixedDeltaTime));
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if(distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }
    }

    private void Shoot()
    {
        if(firingBullets)
        {
            Instantiate(bullet, bulletPosition1.position, Quaternion.identity);
            Instantiate(bullet, bulletPosition2.position, Quaternion.identity);
        }
        else
        {
            Instantiate(missile, bulletPosition1.position, Quaternion.identity);
        }
        
    }

    public void TakeDamage(float damage)
    {
        if(!canTakeDamage) return;
        float filteredDamage = damage;
        if(player != null)
        {
            float damageMultiplier = player.GetDamageMultipler();
            if(damageMultiplier == 100f)
            {
                filteredDamage /= damageMultiplier;
            }
        }
        currentHealth -= filteredDamage;
        
        if(currentHealth <= (MAX_HEALTH/2) && !alreadySpawned)
        {
            StartCoroutine(InvinciblePhase());
        }
        if(currentHealth <= (MAX_HEALTH/4))
        {
            firingBullets = true;
        }
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    

    private void DropCoins()
    {

        if(CoinPoolingAPI.SharedInstance != null)
        {
            for(int i = 0; i < coinsDropped; i++)
            {
                Vector3 randomJitter = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0);
                CoinScript coin = CoinPoolingAPI.SharedInstance.GetPooledCoin();
                coin.transform.position = transform.position + randomJitter;
                coin.gameObject.SetActive(true);
            }
            
        }
        else
        {
            Debug.LogError("CoinPoolingAPI is null in BossScript");
        }
    
    }
    private void Die()
    {
        DropCoins();
        if(StoryManager.Instance != null)
        {
            StoryManager.Instance.DecreaseEnemyCount();
        }
        if(animator != null)
        {
            animator.SetTrigger(DEATH);
        }
        else
        {
            Debug.LogError("Animator is null in BossScript");
        }
        if(explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Explosion is null in BossScript");
        }
        float timer = 0;
        while(timer < 1.5f)
        {
            timer += Time.deltaTime;
        }
        Destroy(gameObject);
    }

    IEnumerator InvinciblePhase()
    {
        alreadySpawned = true;
        canTakeDamage = false;
        if(rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        
        if(invincibleSkin != null && defaultSkin != null)
        {
            invincibleSkin.gameObject.SetActive(true);
            defaultSkin.gameObject.SetActive(false);
        }

       
        // if(OnSpawnEnemies != null)
        // {
        //     OnSpawnEnemies?.Invoke();
        // }
        // else
        // {
        //     Debug.LogError("OnSpawnEnemies is null in BossScript");
        // }

        if(StoryManager.Instance != null)
        {
            StoryManager.Instance.SpawnMinions();
            
        }
        else
        {
            Debug.LogError("StoryManager is null in BossScript");
        }

        float targetHealth = currentHealth + 100f;
        while(currentHealth < targetHealth)
        {
            yield return new WaitForSeconds(2f);
            currentHealth += 10f;
        }
        if(invincibleSkin != null && defaultSkin != null)
        {
            invincibleSkin.gameObject.SetActive(false);
            defaultSkin.gameObject.SetActive(true);
        }
        if(rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        canTakeDamage = true;

        firingBullets = false;
    }


    
}
