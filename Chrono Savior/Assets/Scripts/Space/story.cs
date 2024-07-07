
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] private GameObject fighterShipPrefab;
    [SerializeField] private GameObject bomberShipPrefab;
    [SerializeField] private GameObject sniperShipPrefab;

    [SerializeField] private Text waveText;
    private int[] fighterShipsPerWave = { 3, 2, 0, 2, 0 };
    private int[] bomberShipsPerWave = { 0, 0, 3, 3, 5 };
    private int[] sniperShipsPerWave = { 0, 3, 2, 2, 0 };
    private int currentWave = 0; // Current wave index
    private int enemiesRemaining; // Number of enemies remaining in the current wave
    private float minSpacing = 0.5f; // Minimum spacing between ships
    private List<float> spawnedPositions = new List<float>(); // List to keep track of spawned positions
    private List<GameObject> spawnedEnemies = new List<GameObject>(); // List to keep track of spawned enemies
    private Coroutine asteroidSpawner;
    private Coroutine enemiesSpawner;
    private SpaceGameManager gameManager;

    float screenHeight;

    float minY, maxY, shipSize, aesterpodSize;

    private void Start()
    {
        Camera mainCamera = Camera.main;
        gameManager = FindObjectOfType<SpaceGameManager>();
        if (mainCamera != null)
        {
            minY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + 0.3f;
            maxY = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).y - 0.3f;
            screenHeight = mainCamera.orthographicSize * 2.0f * 0.225f;
        }
        else
        {
            Debug.LogWarning("Main camera is not assigned.");
        }
        shipSize = GetPrefabSize(fighterShipPrefab);
        aesterpodSize = shipSize;
        StartNextWave();
    }

    float GetPrefabSize(GameObject prefab)
    {
        if (prefab != null)
        {
            GameObject tempObject = PoolManager.Instance.SpawnFromPool("FighterShip", transform.position, Quaternion.identity);
            if (tempObject != null)
            {
                SpriteRenderer spriteRenderer = tempObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    float size = (spriteRenderer.bounds.size.y) / 2;
                    tempObject.SetActive(false);
                    return size;
                }
                else
                {
                    Debug.LogWarning("SpriteRenderer not found on tempObject.");
                }
            }
            else
            {
                Debug.LogWarning("TempObject is null.");
            }
        }
        else
        {
            Debug.LogWarning("Prefab is null.");
        }
        return 0f;
    }

    private void StartNextWave()
    {
        if (waveText != null)
        {
            switch (MainMenu.mode)
            {
                case MainMenu.Mode.Campaign:
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
                        if (gameManager != null)
                        {
                            gameManager.WinGame();
                        }
                        else
                        {
                            Debug.LogWarning("GameManager not found.");
                        }
                    }
                    break;
                case MainMenu.Mode.Infinity:
                    enemiesRemaining = 3 + currentWave;
                    int fighterCount = Mathf.RoundToInt(enemiesRemaining * GetSpawnPercentage("fighter"));
                    int bomberCount = Mathf.RoundToInt(enemiesRemaining * GetSpawnPercentage("bomber"));
                    int sniperCount = Mathf.RoundToInt(enemiesRemaining * GetSpawnPercentage("sniper"));
                    asteroidSpawner = StartCoroutine(SpawnAsteroids());
                    enemiesSpawner = StartCoroutine(SpawnWave(fighterCount, bomberCount, sniperCount));
                    break;
                default:
                    Debug.LogWarning("Unknown game mode.");
                    break;
            }
        }
        else
        {
            Debug.LogWarning("WaveText is not assigned.");
        }
    }

    private IEnumerator SpawnWave(int fighterShips, int bomberShips, int sniperShips)
    {
        spawnedPositions.Clear(); // Clear the list of positions for the new wave
        if (waveText != null)
        {
            waveText.text = "Wave " + (currentWave + 1) + " starting...";
            waveText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("WaveText is not assigned.");
        }
        yield return new WaitForSeconds(3.0f);
        if (waveText != null)
        {
            waveText.gameObject.SetActive(false);
        }

        // Spawn fighter ships
        for (int i = 0; i < fighterShips; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition();
            if (spawnPosition.z == -10f)
            {
                yield return new WaitForSeconds(5f);
                i -= 1;
                continue;
            }
            GameObject enemy = PoolManager.Instance.SpawnFromPool("FighterShip", spawnPosition, Quaternion.Euler(0, 0, 90));
            if (enemy != null)
            {
                spawnedEnemies.Add(enemy);
                spawnedPositions.Add(spawnPosition.y); // Add position to the list
            }
            else
            {
                Debug.LogWarning("Failed to spawn FighterShip.");
            }
            yield return new WaitForSeconds(0.5f); // Adjust spawn rate as needed
        }

        // Spawn bomber ships
        for (int i = 0; i < bomberShips; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition();
            if (spawnPosition.z == -10f)
            {
                yield return new WaitForSeconds(5f);
                i -= 1;
                continue;
            }
            GameObject enemy = PoolManager.Instance.SpawnFromPool("BomberShip", spawnPosition, Quaternion.Euler(0, 0, 90));
            if (enemy != null)
            {
                spawnedEnemies.Add(enemy);
                spawnedPositions.Add(spawnPosition.y); // Add position to the list
            }
            else
            {
                Debug.LogWarning("Failed to spawn BomberShip.");
            }
            yield return new WaitForSeconds(0.5f); // Adjust spawn rate as needed
        }

        // Spawn sniper ships
        for (int i = 0; i < sniperShips; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition();
            if (spawnPosition.z == -10f)
            {
                yield return new WaitForSeconds(5f);
                i -= 1;
                continue;
            }
            GameObject enemy = PoolManager.Instance.SpawnFromPool("SniperShip", spawnPosition, Quaternion.Euler(0, 0, 90));
            if (enemy != null)
            {
                spawnedEnemies.Add(enemy);
                spawnedPositions.Add(spawnPosition.y); // Add position to the list
            }
            else
            {
                Debug.LogWarning("Failed to spawn SniperShip.");
            }
            yield return new WaitForSeconds(0.5f); // Adjust spawn rate as needed
        }

        // Wait until all enemies are destroyed
        while (enemiesRemaining > 0)
        {
            yield return null;
        }

        // Stop spawning asteroids when the wave ends
        if (asteroidSpawner != null)
        {
            StopCoroutine(asteroidSpawner);
        }

        // Move to the next wave
        currentWave++;
        yield return new WaitForSeconds(2.0f); // Delay before starting next wave
        StartNextWave();
    }

    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.5f); // Wait for 1 second before spawning the next asteroid
            // Spawn asteroid
            Vector3 spawnPosition = new Vector3(8.0f, Random.Range(minY + aesterpodSize, maxY - aesterpodSize), 0f);
            PoolManager.Instance.SpawnFromPool("Aesteroid", spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(2.5f);
            spawnPosition = new Vector3(8.0f, Random.Range(minY + aesterpodSize, maxY - aesterpodSize), 0f);
            PoolManager.Instance.SpawnFromPool("Mine", spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 CalculateSpawnPosition()
    {
        Vector3 spawnPosition;
        bool validPosition;
        int count = 0;
        do
        {
            validPosition = true;
            float spawnX = 8.0f;
            float spawnY = Random.Range(minY + shipSize, maxY - shipSize - screenHeight);
            spawnPosition = new Vector3(spawnX, spawnY, 0f);
            foreach (float pos in spawnedPositions)
            {
                if (Mathf.Abs(spawnPosition.y - pos) < minSpacing)
                {
                    validPosition = false;
                    break;
                }
            }
            count += 1;
        }
        while (!validPosition && count < 5);
        if (!validPosition)
        {
            return new Vector3(0f, 0f, -10.0f);
        }
        return spawnPosition;
    }

    public void EnemyDestroyed(Vector3 position)
    {
        if (MainMenu.Mode.Infinity == MainMenu.mode)
        {
            if (StateManagement.Instance != null)
            {
                StateManagement.Instance.SetSpaceKillCount(StateManagement.Instance.GetSpaceKillCount() + 1);
                Debug.Log(StateManagement.Instance.GetSpaceKillCount());
            }
            else
            {
                Debug.LogError("StateManagement is not assigned.");
            }
        }
        enemiesRemaining--;
        if (spawnedPositions.Contains(position.y))
        {
            spawnedPositions.Remove(position.y);
        }
    }

    public void DestroyAllEnemies()
    {
        if (asteroidSpawner != null)
        {
            StopCoroutine(asteroidSpawner);
        }

        if (enemiesSpawner != null)
        {
            StopCoroutine(enemiesSpawner);
        }

        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Hazard") || obj.layer == LayerMask.NameToLayer("EnemyBullets") || obj.layer == LayerMask.NameToLayer("Enemy"))
            {
                obj.SetActive(false);
            }
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
