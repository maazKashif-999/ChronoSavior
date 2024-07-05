using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class MeleeRobot : MonoBehaviour, IEnemy
{
    // This script contains the pathfinding of the robots (similar code is used for other enemies). We have used the A* pathfinding package to implement the pathfinding. However, since we have not studied pathfinding in class yet, nor are we entirely familiar with the code, we have largely referenced Brackeys tutorial for pathfinding. 
    [SerializeField] private List<OnPowerupInteract> powerUps = new List<OnPowerupInteract>();
    [SerializeField] private float MAX_HEALTH;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackDamage;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Seeker seeker;
    private float currentHealth;
    private GameObject playerCenter;
    private Player player;
    private float nextWaypointDistance = 0.1f;
    private Path path;
    private int currentWayPoint = 0;
    private float timer = 0;
    private bool isInfinite = false;
    private const float TIME_BETWEEN_ATTACK = 0.5f;
    private const string UPDATE_PATH = "UpdatePath";
    private const string PLAYER_CENTER = "PlayerCenter";
    private const string IS_ATTACKING = "IsAttacking";


    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        playerCenter = GameObject.FindGameObjectWithTag(PLAYER_CENTER);
        currentHealth = MAX_HEALTH;
        if(seeker != null && playerCenter != null)
        {
            InvokeRepeating(UPDATE_PATH,0f,0.5f);
            seeker.StartPath(rb.position, playerCenter.transform.position, OnPathComplete);
        }
        else
        {
            Debug.LogError("Seeker or PlayerCenter is null in MeleeRobot");
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

    void UpdatePath()
    {
        if(seeker == null)
        {
            Debug.Log("Seeker is null in MeleeRobot");
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
    // Update is called once per frame
    void Update()
    {
        if(player == null || playerCenter == null || rb == null)
        {
            Debug.LogError("Player, PlayerCenter or Rigidbody2D is null in MeleeRobot");
            return;
        }

        if (player.AreEnemiesFrozen() && rb.velocity != Vector2.zero)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        timer += Time.deltaTime;
        if(timer > TIME_BETWEEN_ATTACK)
        {
            MeleeAttack();
            timer = 0;
        }
    }
    void FixedUpdate()
    {
        if(player == null || rb == null)
        {
            Debug.LogError("Player or Rigidbody2D is null in MeleeRobot");
            return;
        }
        if(path == null || player.AreEnemiesFrozen()) return;

        if(currentWayPoint >= path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
       
        rb.MovePosition(rb.position + (direction * movementSpeed * Time.fixedDeltaTime));
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        
        if(distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void MeleeAttack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, attackRadius, playerLayer);
        foreach(Collider2D playerCollision in hitPlayer)
        {
            if(animator != null)
            {   
                animator.SetTrigger(IS_ATTACKING);
            }
            else
            {
                Debug.LogError("Animator is null in MeleeRobot");
            }
            if(playerCollision != null)
            {
                playerCollision.GetComponent<Player>().TakeDamage(attackDamage);
            }
            
        }
    }
    private void SpawnPowerUp() // try making this a global function
    {
        int index = Random.Range(0, powerUps.Count);
        if(powerUps[index] == null)
        {
            Debug.LogError("PowerUp is null in GunRobot");
            return;
        }
        if(PowerupPoolingAPI.SharedInstance != null)
        {
            OnPowerupInteract powerup = PowerupPoolingAPI.SharedInstance.GetPooledPowerup(index);
            powerup.transform.position = transform.position;
            powerup.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("PowerupPoolingAPI is null in GunRobot");
        }
    }
    
    private void SpawnCoin()
    {
        if(CoinPoolingAPI.SharedInstance != null)
        {
            CoinScript coin = CoinPoolingAPI.SharedInstance.GetPooledCoin();
            coin.transform.position = transform.position;
            coin.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("CoinPoolingAPI is null in GunRobot");
        }
    
    }
    private void Die()
    {
        int randomNumber = 0;
        if(MAX_HEALTH == 60)
        {
            randomNumber = Random.Range(0, 6);
            if(randomNumber == 0)
            {
                SpawnPowerUp();
            }
            else if(randomNumber == 1)
            {
                SpawnCoin();
            }
        }
        else if(MAX_HEALTH == 50)
        {
            randomNumber = Random.Range(0, 9);
            if(randomNumber == 0)
            {
                SpawnPowerUp();
            }
            else if(randomNumber == 1)
            {
                if(!isInfinite)
                {
                    SpawnCoin();
                }
            }

        }
        Destroy(gameObject);
    }

}
