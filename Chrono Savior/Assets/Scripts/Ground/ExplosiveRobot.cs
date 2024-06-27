using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class ExplosiveRobot : MonoBehaviour, IEnemy
{
    [SerializeField] private List<OnPowerupInteract> powerUps = new List<OnPowerupInteract>();
    [SerializeField] private float MAX_HEALTH = 30f;
    [SerializeField] private bool isShielded = false;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Seeker seeker;
    private float currentHealth;
    private Player player;
    private GameObject playerCenter;
    private float nextWaypointDistance = 0.1f;
    private Path path;
    private int currentWayPoint = 0;
    private const string UPDATE_PATH = "UpdatePath";
    private const string PLAYER_CENTER = "PlayerCenter";
    private const float EXPLODE_DISTANCE = 1f;

    void Start()
    {
        player = Player.Instance;
        playerCenter = GameObject.FindGameObjectWithTag(PLAYER_CENTER);
        currentHealth = MAX_HEALTH;
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
        if(distanceSquared < (EXPLODE_DISTANCE * EXPLODE_DISTANCE))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Die();
        }
    }
    void FixedUpdate()
    {
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

    private void SpawnPowerUp()
    {
        int index = Random.Range(0, powerUps.Count);
        Instantiate(powerUps[index], transform.position, Quaternion.identity);
    }
    private void Die()
    {
        Destroy(gameObject);
        if(Random.Range(0, 6) == 0)
        {
            SpawnPowerUp();
        }
    }

}
