using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class MeleeRobot : MonoBehaviour, IEnemy
{
    // This script contains the pathfinding of the robots (similar code is used for other enemies). We have used the A* pathfinding package to implement the pathfinding. However, since we have not studied pathfinding in class yet, nor are we entirely familiar with the code, we have largely referenced Brackeys tutorial for pathfinding. 
    [SerializeField] private List<OnPowerupInteract> powerUps;
    [SerializeField] private float MAX_HEALTH = 30f;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackDamage;
    private float currentHealth;
    private GameObject player;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Seeker seeker;
    private float nextWaypointDistance = 0.1f;
    private Path path;
    private int currentWayPoint = 0;
    private float timer = 0;
    private const float TIME_BETWEEN_ATTACK = 0.5f;

    // Start is called before the first frame update
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
    // Update is called once per frame
    void Update()
    {
        if (Player.Instance.AreEnemiesFrozen() && rb.velocity != Vector2.zero)
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
        if(path == null || Player.Instance.AreEnemiesFrozen()) return;

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
        foreach(Collider2D plyer in hitPlayer)
        {
            animator.SetTrigger("IsAttacking");
            plyer.GetComponent<Player>().TakeDamage(attackDamage);
        }
    }
    private void SpawnPowerUp()
    {
        int index = Random.Range(0, powerUps.Count);
        Instantiate(powerUps[index], transform.position, Quaternion.identity);
    }
    private void Die()
    {
        if(MAX_HEALTH > 50)
        {
            if(Random.Range(0,6) % 5 == 0)
            {
                SpawnPowerUp();
            }
        }
        else if(MAX_HEALTH > 30)
        {
            if(Random.Range(0,9) % 8 == 0)
            {
                SpawnPowerUp();
            }
        }
        Destroy(gameObject);
    }

}
