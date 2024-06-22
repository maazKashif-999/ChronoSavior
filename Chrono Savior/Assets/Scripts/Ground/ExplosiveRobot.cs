using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class ExplosiveRobot : MonoBehaviour, IEnemy
{
    public List<OnPowerupInteract> powerUps;
    [SerializeField] private float MAX_HEALTH = 30f;
    [SerializeField] private bool isShielded = false;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject explosion;
    private float currentHealth;
    private GameObject player;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Seeker seeker;
    private float nextWaypointDistance = 0.1f;

    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerCenter");
        currentHealth = MAX_HEALTH;
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
        if(distance < 1f)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Die();
        }
    }
    void FixedUpdate()
    {
        if(path == null || Player.Instance.AreEnemiesFrozen()) return;

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
        int randomInt = Random.Range(0, 6);
        if(randomInt % 5 == 0)
        {
            SpawnPowerUp();
        }
    }

}
