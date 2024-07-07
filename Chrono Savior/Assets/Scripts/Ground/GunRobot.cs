using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GunRobot : GroundEnemyScript, IEnemy
{   
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPosition;
    private const float TIME_BETWEEN_SHOTS = 0.5f;
    private const float SHOOT_DISTANCE = 5f;
    private const float AWAY_DISTANCE = 2f;
    private bool inRange = false;
    private const string IS_SHOOTING = "IsShooting";


    protected override void Update()
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

    protected override void FixedUpdate()
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

    protected override void Die()
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
