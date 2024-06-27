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
        InvokeRepeating(UPDATE_PATH,0f,0.5f);
        seeker.StartPath(rb.position, playerCenter.transform.position, OnPathComplete);
    }

    void UpdatePath()
    {
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
        if (player.AreEnemiesFrozen())
        {
            rb.velocity = Vector2.zero;
            return;
        }
        Vector2 distanceVec = playerCenter.transform.position - transform.position;
        float distanceSquared = distanceVec.sqrMagnitude;
        if(distanceSquared <= (SHOOT_DISTANCE * SHOOT_DISTANCE))
        {
            if(distanceSquared <= (AWAY_DISTANCE * AWAY_DISTANCE)) 
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
        if(path == null || inRange || player.AreEnemiesFrozen()) return;
        
        if(currentWayPoint >= path.vectorPath.Count)
        {
            return;
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
        animator.SetTrigger(IS_SHOOTING);
    }

    private void SpawnPowerUp()
    {
        int index = Random.Range(0, powerUps.Count);
        Instantiate(powerUps[index], transform.position, Quaternion.identity);
    }

    private void Die()
    {
        Destroy(gameObject);
        if(Random.Range(0, 5) == 0)
        {
            SpawnPowerUp();
        }
        
    }

}
