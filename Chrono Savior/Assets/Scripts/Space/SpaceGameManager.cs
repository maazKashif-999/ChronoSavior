using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceGameManager : MonoBehaviour
{
    
    
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemies;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameCompletePanel;

    [SerializeField] private GameObject coinsCollected,tokensCollected;

    private EnemyWaveManager enemyWaveManager;

    public enum GameManagerState
    {
        playing,
        infinity,
        gameover,
        gamewon
    }

    private GameManagerState GMState;

    [SerializeField] private Text timerText;
    [SerializeField] private Text highestScoreText;
    private float elapsedTime;
    private bool isRunning;
    void UpdateTimerUI()
    {
        timerText.text = "Score: " + FormatTime(elapsedTime);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void Start()
    {
        switch(MainMenu.mode)
        {
            case MainMenu.Mode.Infinity:
                StartInfinity();
                break;
            case MainMenu.Mode.Campaign:
                StartGame();
                break;
            default:
                Debug.LogError("Unknown game mode");
                break;

        }
    }

    private void Update()
    {
        if (isRunning){
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
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
                    PlayerControls playerControls = player.GetComponent<PlayerControls>();
                    if(playerControls != null)
                    {
                        int coins_gained = playerControls.GetCoins();
                        if(StateManagement.Instance != null)
                        {
                            int current_coins = StateManagement.Instance.GetCoins();
                            current_coins += coins_gained;
                            StateManagement.Instance.SetCoins(coins_gained);
                        }
                        else
                        {
                            Debug.LogError("StateManagement is not assigned.");
                        }
                        if (StateManagement.Instance.GetSpaceKillCount() >= 15){
                            AchievementManager.Instance.CheckLocked("Starship Destroyer");
                        }
                        
                        
                    }
                    if(gameCompletePanel != null)
                    {
                        gameCompletePanel.SetActive(false);
                    }
                    else
                    {
                        Debug.LogWarning("GameCompletePanel GameObject not assigned in SpaceGameManager.");
                    
                    }
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
                if(player != null)
                {
                    Debug.Log("In game won");

                    PlayerControls playerControls = player.GetComponent<PlayerControls>();
                    if(playerControls != null)
                    {
                        int coins_gained = playerControls.GetCoins();
                        int tokens_gained = playerControls.GetTokens();
                        if (StateManagement.Instance != null)
                        {
                            int current_coins = StateManagement.Instance.GetCoins();
                            current_coins += coins_gained;
                            StateManagement.Instance.SetCoins(current_coins);
                            StateManagement.Instance.SetTokens(tokens_gained);
                            StateManagement.Instance.SetSessionLoad(true);
                            
                        }
                        else
                        {
                            Debug.LogError("StateManagement is not assigned.");
                        }
                    }

                    if (gameCompletePanel != null)
                    {
                        gameCompletePanel.SetActive(true);
                    }

                    else
                    {
                        Debug.LogWarning("GameCompletePanel GameObject not assigned in SpaceGameManager.");

                    }
                }
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
        coinsCollected.SetActive(true);
        tokensCollected.SetActive(true);
        timerText.gameObject.SetActive(false);
        highestScoreText.gameObject.SetActive(false);
        SetGMState(GameManagerState.playing);
    }

    public void StartInfinity()
    {
        isRunning = true;
        elapsedTime = 0f;
        timerText.gameObject.SetActive(true);
        highestScoreText.gameObject.SetActive(true);
        if(StateManagement.Instance != null)
        {
            highestScoreText.text = "Highest Score: " + FormatTime(StateManagement.Instance.GetSpaceHighestScore());
        }
        else
        {
            Debug.LogError("StateManagement is not assigned.");
        }
        
        SetGMState(GameManagerState.infinity);
    }

    public void EndGame()
    {
        isRunning = false;
        timerText.gameObject.SetActive(false);
        

        if(StateManagement.Instance != null)
        {
            float highestScore = StateManagement.Instance.GetSpaceHighestScore();
            Debug.Log(elapsedTime);
            if(elapsedTime > highestScore)
            {
                StateManagement.Instance.SetSpaceHighestScore(elapsedTime);
                if (elapsedTime > 1800f )//check if highest score is more than half an hour
                {
                    AchievementManager.Instance.CheckLocked("Survivalist");
                }
            }
        }
        else
        {
            Debug.LogError("StateManagement is not assigned.");
            
        }

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
        Debug.Log("in main gamewon");
        SetGMState(GameManagerState.gamewon);
    }
}
