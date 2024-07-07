using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCompleteScript : MonoBehaviour
{
    [SerializeField] private GameObject gameCompleteCanvas;

    // Start is called before the first frame update
    void Start()
    {
        if (gameCompleteCanvas == null)
        {
            Debug.LogError("GameOverCanvas null.");
        }
        else
        {
            gameCompleteCanvas.SetActive(false);
        }
    }

    public void ShowGameCompleteScreen()
    {
        if(gameCompleteCanvas == null)
        {
            Debug.LogError("GameOverCanvas null.");
            return;
        }
        // Debug.Log("Game Over Screen called"); 
        PauseMenu.gameIsPaused = true;
        gameCompleteCanvas.SetActive(true); 
        Time.timeScale = 0f; 
        

    }


    public void MainMenu()
    {
        PauseMenu.gameIsPaused = false;
        Time.timeScale = 1f; 
        SceneManager.LoadSceneAsync(0);
    }
}
