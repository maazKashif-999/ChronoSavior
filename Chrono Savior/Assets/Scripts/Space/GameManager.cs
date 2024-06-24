using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public GameObject playButton;
    public GameObject player;
    public GameObject enemies;
    public GameObject gameOverPanel;


    private EnemyWaveManager enemyWaveManager;

    public enum GameManagerState{
        opening,
        playing,
        gameover,
        gamewon
    }
    GameManagerState GMState;
    // Start is called before the first frame update
    void Start()
    {
        GMState = GameManagerState.opening;
        UpdateGMState();
    }
    void UpdateGMState()
    {
        switch(GMState){
            case GameManagerState.opening:
            player.SetActive(false);
            gameOverPanel.SetActive(false);
            enemies.SetActive(false);
            break;
            case GameManagerState.playing:
            playButton.SetActive(false);
            player.GetComponent<PlayerControls>().Init();
            enemies.SetActive(true);
            enemyWaveManager = FindObjectOfType<EnemyWaveManager>(); 
            break;
            case GameManagerState.gameover:
            player.SetActive(false);
            gameOverPanel.SetActive(true);
            enemyWaveManager.DestroyAllEnemies();
            break;
            case GameManagerState.gamewon:
            break;
        }
    }

    public void setGMState(GameManagerState state)
    {
        GMState = state;
        UpdateGMState();
    }
    public void startGame()
    {
        GMState = GameManagerState.playing;
        UpdateGMState();
    }
    public void endGame()
    {
        GMState = GameManagerState.gameover;
        UpdateGMState();
    }
    public void RestartGame()
    {
        // Reset player position and health if needed
        player.GetComponent<PlayerControls>().Init();
        // Reset enemy wave manager
        enemyWaveManager.ResetWaves();
        // Hide game over panel
        gameOverPanel.SetActive(false);
        // Start the game
        startGame();
    }

    public void WinGame()
    {
        GMState = GameManagerState.gamewon;
        UpdateGMState();
    }

}