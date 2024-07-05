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
    private int[] fighterShipsPerWave = {3, 2, 0, 2, 0};
    private int[] bomberShipsPerWave = {0, 0, 3, 3, 5};
    private int[] sniperShipsPerWave = {0, 3, 2, 2, 0};
    private int currentWave = 0; // Current wave index
    private int enemiesRemaining; // Number of enemies remaining in the current wave
    private float minSpacing = 0.5f; // Minimum spacing between ships
    private List<Vector3> spawnedPositions = new List<Vector3>(); // List to keep track of spawned positions
    private List<GameObject> spawnedEnemies = new List<GameObject>(); // List to keep track of spawned enemies
    private Coroutine asteroidSpawner; 
    private Coroutine enemiesSpawner;
    private SpaceGameManager gameManager; 

    float minY,maxY,shipSize,aesterpodSize;

    private void Start()
    {
        Camera mainCamera = Camera.main;
        gameManager = FindObjectOfType<SpaceGameManager>();
        if (mainCamera != null)
        {
            
            minY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + 0.3f ;
            maxY = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).y - 0.3f ;
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
        GameObject tempObject = PoolManager.Instance.SpawnFromPool("FighterShip",transform.position, Quaternion.identity);
        SpriteRenderer spriteRenderer = tempObject.GetComponent<SpriteRenderer>();
        float size = (spriteRenderer.bounds.size.y)/2;
        tempObject.SetActive(false);

        return size;
    }
    private void StartNextWave()
    {
        switch (MainMenu.mode){
            case MainMenu.Mode.Campaign:
            if (currentWave < fighterShipsPerWave.Length)
            {
                enemiesRemaining = fighterShipsPerWave[currentWave] + bomberShipsPerWave[currentWave] + sniperShipsPerWave[currentWave];
                asteroidSpawner = StartCoroutine(SpawnAsteroids()); 
                enemiesSpawner = StartCoroutine(SpawnWave(fighterShipsPerWave[currentWave],bomberShipsPerWave[currentWave],sniperShipsPerWave[currentWave]));
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
                enemiesSpawner = StartCoroutine(SpawnWave(fighterCount,bomberCount,sniperCount));
                break;
            default:
                Debug.LogWarning("Unknown game mode.");
                break;
        }
    }
    private IEnumerator SpawnWave(int fighterShips,int bomberShips,int sniperShips)
    {
        spawnedPositions.Clear(); // Clear the list of positions for the new wave
        waveText.text = "Wave " + (currentWave + 1) + " starting...";
        waveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        waveText.gameObject.SetActive(false);
        // Spawn fighter ships
        for (int i = 0; i < fighterShips; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition();
            GameObject enemy = PoolManager.Instance.SpawnFromPool("FighterShip", spawnPosition, Quaternion.Euler(0, 0, 90));
            spawnedEnemies.Add(enemy);
            spawnedPositions.Add(spawnPosition); // Add position to the list
            yield return new WaitForSeconds(0.5f); // Adjust spawn rate as needed
        }
        // Spawn bomber ships
        for (int i = 0; i < bomberShips; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition();
            GameObject enemy = PoolManager.Instance.SpawnFromPool("BomberShip", spawnPosition,Quaternion.Euler(0, 0, 90));
            spawnedEnemies.Add(enemy);
            spawnedPositions.Add(spawnPosition); // Add position to the list
            yield return new WaitForSeconds(0.5f); // Adjust spawn rate as needed
        }
        // Spawn sniper ships
        for (int i = 0; i < sniperShips; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition();
            GameObject enemy = PoolManager.Instance.SpawnFromPool("SniperShip", spawnPosition, Quaternion.Euler(0, 0, 90));
            spawnedEnemies.Add(enemy);
            spawnedPositions.Add(spawnPosition); // Add position to the list
            yield return new WaitForSeconds(0.5f); // Adjust spawn rate as needed
        }
        // Wait until all enemies are destroyed
        while (enemiesRemaining > 0)
        {
            yield return null;
        }
        // Stop spawning asteroids when the wave ends
        StopCoroutine(asteroidSpawner);
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
            Vector3 spawnPosition = new Vector3(8.0f, Random.Range(minY+aesterpodSize, maxY-aesterpodSize), 0f);
            PoolManager.Instance.SpawnFromPool("Aesteroid", spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(2.5f);
            spawnPosition = new Vector3(8.0f, Random.Range(minY+aesterpodSize, maxY-aesterpodSize), 0f);
            PoolManager.Instance.SpawnFromPool("Mine", spawnPosition, Quaternion.identity);
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
            float offsetFromUI = 0.5f;
            float spawnY = Random.Range(minY+shipSize, maxY-shipSize - offsetFromUI);
            spawnPosition = new Vector3(spawnX, spawnY, 0f);
            // Check if the new spawn position is too close to any existing positions
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
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Hazard") || obj.layer == LayerMask.NameToLayer("EnemyBullets") || obj.layer == LayerMask.NameToLayer("Enemy"))
            {
                // PoolManager.Instance.ReturnToPool(obj.tag, obj);
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