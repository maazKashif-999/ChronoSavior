using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ExplosiveRobot : GroundEnemyScript, IEnemy
{
    [SerializeField] private GameObject explosion;
    private const float EXPLODE_DISTANCE = 1f;

    protected override void Start()
    {
        nextWaypointDistance = 0.1f;
        base.Start();
    }


    protected override void Update()
    {
        if (player == null || playerCenter == null || rb == null)
        {
            Debug.LogError("Player, PlayerCenter or Rigidbody2D is null in ExplosiveRobot");
            return;
        }

        if (player.AreEnemiesFrozen())
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 distanceVec = playerCenter.transform.position - transform.position;
        float distanceSquared = distanceVec.sqrMagnitude;
        if (distanceSquared < (EXPLODE_DISTANCE * EXPLODE_DISTANCE))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Die();
        }
    }

    protected override void FixedUpdate()
    {
        if (player == null || rb == null)
        {
            Debug.LogError("Player or Rigidbody2D is null in ExplosiveRobot");
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

    protected override void SpawnPowerUp()
    {
        if (powerUps.Count == 0)
        {
            Debug.LogWarning("No power-ups assigned in ExplosiveRobot.");
            return;
        }

        int index = Random.Range(0, powerUps.Count);
        if (powerUps[index] == null)
        {
            Debug.LogError("PowerUp is null in ExplosiveRobot");
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
            Debug.LogError("PowerupPoolingAPI is null in ExplosiveRobot");
        }
    }


    protected override void Die()
    {
        Destroy(gameObject);
        int randomNumber = Random.Range(0, 6);
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
        if (!isInfinite && StoryManager.Instance != null)
        {
            StoryManager.Instance.DecreaseEnemyCount();
        }
        if (isInfinite && StateManagement.Instance != null)
        {
            StateManagement.Instance.SetGroundKillCount(StateManagement.Instance.GetGroundKillCount() + 1);
        }
    }
}
