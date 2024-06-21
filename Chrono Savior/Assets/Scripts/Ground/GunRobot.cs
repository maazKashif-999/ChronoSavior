using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class GunRobot : MonoBehaviour, IEnemy
{
    public List<OnPowerupInteract> powerUps;
    private System.Random random;
    [SerializeField] private float MAX_HEALTH = 70f;
    [SerializeField] private Animator animator;
    private float currentHealth;
    private GameObject player;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Seeker seeker;
    private float nextWaypointDistance = 0.5f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPosition;


    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;
    private float timer = 0;
    private const float TIME_BETWEEN_SHOTS = 0.5f;
    private const float SHOOT_DISTANCE = 5f;
    private const float AWAY_DISTANCE = 2f;
    private bool inRange = false;

    
    void Start()
    {
        currentHealth = MAX_HEALTH;
        random = new System.Random();
        player = GameObject.FindGameObjectWithTag("PlayerCenter");
        InvokeRepeating("UpdatePath",0f,0.5f);
        seeker.StartPath(rb.position, player.transform.position, OnPathComplete);
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
        {
            seeker.StartPath(rb.position, player.transform.position, OnPathComplete);
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
        if (Player.Instance.AreEnemiesFrozen())
        {
            rb.velocity = Vector2.zero;
            return;
        }
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if(distance <= SHOOT_DISTANCE)
        {
            if(distance <= AWAY_DISTANCE)
            {
                inRange = true;
            }
            else
            {
                inRange = false;
            }
            
            rb.velocity = new Vector3(0,0,0);
            timer += Time.deltaTime;
            if(timer > TIME_BETWEEN_SHOTS)
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
        if(path == null || inRange || Player.Instance.AreEnemiesFrozen()) return;
        
        if(currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

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

    public void TakeDamage(float damage)
    {

        currentHealth -= damage;
        
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Shoot()
    {
        Instantiate(bullet, bulletPosition.position, Quaternion.identity);
        animator.SetTrigger("IsShooting");
    }

    private void SpawnPowerUp()
    {
        float totalProbability = 0f;
        foreach (var powerUp in powerUps)
        {
            totalProbability += powerUp.probability;
        }

        float randomPoint = Random.value * totalProbability;

        foreach (var powerUp in powerUps)
        {
            if (randomPoint < powerUp.probability)
            {
                Instantiate(powerUp, transform.position, Quaternion.identity);
                return;
            }
            else
            {
                randomPoint -= powerUp.probability;
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        int randomInt = random.Next(0, 11);
        if(randomInt % 10 == 0)
        {
            SpawnPowerUp();
        }
        
    }

}
