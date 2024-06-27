using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGameManager : MonoBehaviour
{
    public static SpaceGameManager Instance { get; private set; }
    
    public GameObject player;
    public GameObject enemies;
    public GameObject gameOverPanel;

    private EnemyWaveManager enemyWaveManager;

    public enum GameManagerState
    {
        playing,
        infinity,
        gameover,
        gamewon
    }

    private GameManagerState GMState;

    private void Awake()
    {
        // Ensure that only one instance of the GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GMState = GameManagerState.playing;
        UpdateGMState();
    }

    void UpdateGMState()
    {
        switch (GMState)
        {
            case GameManagerState.playing:
                if (player != null)
                {
                    PlayerControls playerControls = player.GetComponent<PlayerControls>();
                    if (playerControls != null)
                    {
                        playerControls.Init();
                    }
                    else
                    {
                        Debug.LogWarning("PlayerControls component not found on player.");
                    }
                }
                else
                {
                    Debug.LogWarning("Player GameObject not assigned in SpaceGameManager.");
                }

                if (enemies != null)
                {
                    enemies.SetActive(true);
                    enemyWaveManager = FindObjectOfType<EnemyWaveManager>();
                    if (enemyWaveManager == null)
                    {
                        Debug.LogWarning("EnemyWaveManager not found in the scene.");
                    }
                }
                else
                {
                    Debug.LogWarning("Enemies GameObject not assigned in SpaceGameManager.");
                }
                break;

            case GameManagerState.infinity:
                if (player != null)
                {
                    PlayerControls playerControls = player.GetComponent<PlayerControls>();
                    if (playerControls != null)
                    {
                        playerControls.Init();
                    }
                    else
                    {
                        Debug.LogWarning("PlayerControls component not found on player.");
                    }
                }
                else
                {
                    Debug.LogWarning("Player GameObject not assigned in SpaceGameManager.");
                }

                if (enemies != null)
                {
                    enemies.SetActive(true);
                    enemyWaveManager = FindObjectOfType<EnemyWaveManager>();
                    
                }
                else
                {
                    Debug.LogWarning("Enemies GameObject not assigned in SpaceGameManager.");
                }
                break;

            case GameManagerState.gameover:
                if (player != null)
                {
                    player.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("Player GameObject not assigned in SpaceGameManager.");
                }

                if (gameOverPanel != null)
                {
                    gameOverPanel.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("GameOverPanel GameObject not assigned in SpaceGameManager.");
                }

                if (enemyWaveManager != null)
                {
                    enemyWaveManager.DestroyAllEnemies();
                }
                else
                {
                    Debug.LogWarning("EnemyWaveManager not found or not initialized.");
                }
                break;

            case GameManagerState.gamewon:
                // Implement what happens when the game is won
                break;
        }
    }

    public void SetGMState(GameManagerState state)
    {
        GMState = state;
        UpdateGMState();
    }

    public void StartGame()
    {
        SetGMState(GameManagerState.playing);
    }

    public void StartInfinity()
    {
        SetGMState(GameManagerState.infinity);
    }

    public void EndGame()
    {
        SetGMState(GameManagerState.gameover);
    }

    public void RestartGame()
    {
        if (player != null)
        {
            PlayerControls playerControls = player.GetComponent<PlayerControls>();
            if (playerControls != null)
            {
                playerControls.Init();
            }
            else
            {
                Debug.LogWarning("PlayerControls component not found on player.");
            }
        }
        else
        {
            Debug.LogWarning("Player GameObject not assigned in SpaceGameManager.");
        }

        if (enemyWaveManager != null)
        {
            enemyWaveManager.ResetWaves();
        }
        else
        {
            Debug.LogWarning("EnemyWaveManager not found or not initialized.");
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GameOverPanel GameObject not assigned in SpaceGameManager.");
        }

        switch (MainMenu.mode)
    {
        case MainMenu.Mode.Campaign:
            StartGame();
            break;

        case MainMenu.Mode.Infinity:
            StartInfinity();
            break;

        default:
            Debug.LogWarning("Unknown game mode.");
            break;
    }
    }

    public void WinGame()
    {
        SetGMState(GameManagerState.gamewon);
    }
}
