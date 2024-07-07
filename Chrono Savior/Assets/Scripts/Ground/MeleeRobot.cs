using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeRobot : GroundEnemyScript, IEnemy
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackDamage;

    private const float TIME_BETWEEN_ATTACK = 0.5f;
    private const string IS_ATTACKING = "IsAttacking";

    protected override void Start()
    {
        nextWaypointDistance = 0.1f;
        base.Start();
    }



    protected override void Update()
    {
        if (player == null || playerCenter == null || rb == null)
        {
            Debug.LogError("Player, PlayerCenter or Rigidbody2D is null in MeleeRobot.");
            return;
        }

        if (player.AreEnemiesFrozen() && rb.velocity != Vector2.zero)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        else if (player.AreEnemiesFrozen())
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer > TIME_BETWEEN_ATTACK)
        {
            MeleeAttack();
            timer = 0;
        }
    }

    protected override void FixedUpdate()
    {
        if (player == null || rb == null)
        {
            Debug.LogError("Player or Rigidbody2D is null in MeleeRobot.");
            return;
        }

        if (path == null || player.AreEnemiesFrozen()) return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        rb.MovePosition(rb.position + (direction * movementSpeed * Time.fixedDeltaTime));

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }
    }

    public void MeleeAttack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, attackRadius, playerLayer);
        foreach (Collider2D playerCollision in hitPlayer)
        {
            if (animator != null)
            {
                animator.SetTrigger(IS_ATTACKING);
            }
            else
            {
                Debug.LogError("Animator is null in MeleeRobot.");
            }

            if (playerCollision != null)
            {
                Player playerComponent = playerCollision.GetComponent<Player>();
                if (playerComponent != null)
                {
                    playerComponent.TakeDamage(attackDamage);
                }
                else
                {
                    Debug.LogError("Player component is null in MeleeRobot.");
                }
            }
        }
    }



    protected override void Die()
    {
        int randomNumber = 0;
        if (MAX_HEALTH == 60)
        {
            randomNumber = Random.Range(0, 6);
            if (randomNumber == 0)
            {
                SpawnPowerUp();
            }
            else if (randomNumber == 1)
            {
                SpawnCoin();
            }
        }
        else if (MAX_HEALTH == 50)
        {
            randomNumber = Random.Range(0, 9);
            if (randomNumber == 0)
            {
                SpawnPowerUp();
            }
            else if (randomNumber == 1)
            {
                if (!isInfinite)
                {
                    SpawnCoin();
                }
            }
        }

        if (!isInfinite && StoryManager.Instance != null)
        {
            StoryManager.Instance.DecreaseEnemyCount();
        }

        if (isInfinite && StateManagement.Instance != null)
        {
            StateManagement.Instance.SetGroundKillCount(StateManagement.Instance.GetGroundKillCount() + 1);
            Debug.Log(StateManagement.Instance.GetGroundKillCount());
        }
        Destroy(gameObject);
    }
}
