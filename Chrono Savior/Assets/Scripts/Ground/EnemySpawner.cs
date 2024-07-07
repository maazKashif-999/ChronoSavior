using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject basicRobotPrefab;
    [SerializeField] private GameObject shieldedRobotPrefab;
    [SerializeField] private GameObject gunRobotPrefab;
    [SerializeField] private GameObject explosiveRobotPrefab;
    [SerializeField] private GameObject heavyRobotPrefab;
    [SerializeField] private GameObject securityTurretPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private int currentWave = 1;
    private int enemiesToSpawn;
    private float baseWaitTime = 15f;
    private float waitTimeIncrement = 2f;

    void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn points are not assigned in EnemySpawner.");
            return;
        }

        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (true)
        {
            enemiesToSpawn = CalculateEnemiesToSpawn(currentWave);
            SpawnEnemiesForWave(currentWave);
            yield return new WaitForSeconds(baseWaitTime + waitTimeIncrement * (currentWave - 1));
            currentWave++;
        }
    }

    private int CalculateEnemiesToSpawn(int wave)
    {
        float funcValue = wave * (wave + 1) / 4.0f;
        int enemies = (int)Math.Ceiling(funcValue);
        return enemies;
    }

    private void SpawnEnemiesForWave(int wave)
    {
        List<GameObject> enemies = new List<GameObject>();

        if (wave >= 8)
        {
            AddEnemies(enemies, basicRobotPrefab, enemiesToSpawn * 20 / 100);
            AddEnemies(enemies, shieldedRobotPrefab, enemiesToSpawn * 20 / 100);
            AddEnemies(enemies, gunRobotPrefab, enemiesToSpawn * 20 / 100);
            AddEnemies(enemies, explosiveRobotPrefab, enemiesToSpawn * 20 / 100);
            AddEnemies(enemies, heavyRobotPrefab, enemiesToSpawn * 20 / 100);
        }
        else if (wave >= 6)
        {
            AddEnemies(enemies, basicRobotPrefab, enemiesToSpawn * 40 / 100);
            AddEnemies(enemies, shieldedRobotPrefab, enemiesToSpawn * 30 / 100);
            AddEnemies(enemies, gunRobotPrefab, enemiesToSpawn * 20 / 100);
            AddEnemies(enemies, explosiveRobotPrefab, enemiesToSpawn * 10 / 100);
        }
        else if (wave >= 4)
        {
            AddEnemies(enemies, basicRobotPrefab, enemiesToSpawn * 60 / 100);
            AddEnemies(enemies, shieldedRobotPrefab, enemiesToSpawn * 30 / 100);
            AddEnemies(enemies, gunRobotPrefab, enemiesToSpawn * 10 / 100);
        }
        else if (wave >= 2)
        {
            AddEnemies(enemies, basicRobotPrefab, enemiesToSpawn * 80 / 100);
            AddEnemies(enemies, shieldedRobotPrefab, enemiesToSpawn * 20 / 100);
        }
        else
        {
            AddEnemies(enemies, basicRobotPrefab, enemiesToSpawn);
        }

        SpawnAtPoints(enemies);
    }

    private void AddEnemies(List<GameObject> enemies, GameObject prefab, int count)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Attempted to add enemies with a null prefab.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            enemies.Add(prefab);
        }
    }

    private void SpawnAtPoints(List<GameObject> enemies)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned in EnemySpawner.");
            return;
        }

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
                Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                Debug.LogWarning("Attempted to spawn a null enemy.");
            }
        }

        if (securityTurretPrefab != null)
        {
            if (UnityEngine.Random.value > 0.5f)
            {
                Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
                Instantiate(securityTurretPrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}
