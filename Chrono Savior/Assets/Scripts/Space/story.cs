using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyWaveManager : MonoBehaviour
{
    public GameObject fighterShipPrefab;
    public GameObject bomberShipPrefab;
    public GameObject sniperShipPrefab;
    public GameObject asteroidPrefab;
    public TextMeshProUGUI waveText;

    private int[] fighterShipsPerWave = { 3, 2, 0, 2, 0 };
    private int[] bomberShipsPerWave = { 0, 0, 3, 3, 5 };
    private int[] sniperShipsPerWave = { 0, 3, 2, 2, 0 };

    private int currentWave = 0;
    private int enemiesRemaining;
    public float minSpacing = 0.5f;

    private List<Vector3> spawnedPositions = new List<Vector3>();
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private Coroutine asteroidSpawner;
    private Coroutine enemiesSpawner;

    private Mode mode;

    public enum Mode
    {
        Infinity,
        Campaign
    }

    public Mode GetMode()
    {
        return mode;
    }

    public void SetMode(Mode newMode)
    {
        mode = newMode;
    }

    private void Start()
    {
        StartNextWave();
    }

    private void StartNextWave()
    {
        if (mode == Mode.Campaign)
        {
            if (currentWave < fighterShipsPerWave.Length)
            {
                enemiesRemaining = fighterShipsPerWave[currentWave] + bomberShipsPerWave[currentWave] + sniperShipsPerWave[currentWave];
                asteroidSpawner = StartCoroutine(SpawnAsteroids());
                enemiesSpawner = StartCoroutine(SpawnWave(fighterShipsPerWave[currentWave], bomberShipsPerWave[currentWave], sniperShipsPerWave[currentWave]));
            }
            else
            {
                waveText.gameObject.SetActive(true);
                waveText.text = "All waves cleared!";
            }
        }
        else if (mode == Mode.Infinity)
        {
            enemiesRemaining = 3 + currentWave;
            int fighterCount = Mathf.RoundToInt(enemiesRemaining * GetSpawnPercentage("fighter"));
            int bomberCount = Mathf.RoundToInt(enemiesRemaining * GetSpawnPercentage("bomber"));
            int sniperCount = Mathf.RoundToInt(enemiesRemaining * GetSpawnPercentage("sniper"));

            asteroidSpawner = StartCoroutine(SpawnAsteroids());
            enemiesSpawner = StartCoroutine(SpawnWave(fighterCount, bomberCount, sniperCount));
        }
    }

    private IEnumerator SpawnWave(int fighterShips, int bomberShips, int sniperShips)
    {
        spawnedPositions.Clear();
        waveText.text = "Wave " + (currentWave + 1) + " starting...";
        waveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        waveText.gameObject.SetActive(false);

        for (int i = 0; i < fighterShips; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition();
            GameObject enemy = Instantiate(fighterShipPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(enemy);
            spawnedPositions.Add(spawnPosition);
            yield return new WaitForSeconds(0.5f);
        }

        for (int i = 0; i < bomberShips; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition();
            GameObject enemy = Instantiate(bomberShipPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(enemy);
            spawnedPositions.Add(spawnPosition);
            yield return new WaitForSeconds(0.5f);
        }

        for (int i = 0; i < sniperShips; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition();
            GameObject enemy = Instantiate(sniperShipPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(enemy);
            spawnedPositions.Add(spawnPosition);
            yield return new WaitForSeconds(0.5f);
        }

        while (enemiesRemaining > 0)
        {
            yield return null;
        }

        StopCoroutine(asteroidSpawner);
        currentWave++;
        yield return new WaitForSeconds(2.0f);
        StartNextWave();
    }

    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            Vector3 spawnPosition = new Vector3(8.0f, Random.Range(-3f, 3f), 0f);
            Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 CalculateSpawnPosition()
    {
        Vector3 spawnPosition;
        bool validPosition;

        do
        {
            validPosition = true;
            float spawnX = 8.0f;
            float spawnY = Random.Range(-3f, 3f);
            spawnPosition = new Vector3(spawnX, spawnY, 0f);

            foreach (Vector3 pos in spawnedPositions)
            {
                if (Vector3.Distance(spawnPosition, pos) < minSpacing)
                {
                    validPosition = false;
                    break;
                }
            }
        }
        while (!validPosition);

        return spawnPosition;
    }

    public void EnemyDestroyed()
    {
        enemiesRemaining--;
    }

    public void DestroyAllEnemies()
    {
        StopCoroutine(asteroidSpawner);
        StopCoroutine(enemiesSpawner);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullets");
        foreach (GameObject bullet in enemyBullets)
        {
            Destroy(bullet);
        }

        GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject powerUp in powerUps)
        {
            Destroy(powerUp);
        }
    }

    public void ResetWaves()
    {
        currentWave = 0;
        StartNextWave();
    }

    private float GetSpawnPercentage(string shipType)
    {
        if (currentWave < 3)
        {
            return shipType == "fighter" ? 1.0f : 0.0f;
        }
        else if (currentWave < 6)
        {
            switch (shipType)
            {
                case "fighter":
                    return 0.6f;
                case "bomber":
                    return 0.4f;
                default:
                    return 0.0f;
            }
        }
        else
        {
            switch (shipType)
            {
                case "fighter":
                    return 0.3f;
                case "bomber":
                    return 0.4f;
                case "sniper":
                    return 0.3f;
                default:
                    return 0.0f;
            }
        }
    }
}
