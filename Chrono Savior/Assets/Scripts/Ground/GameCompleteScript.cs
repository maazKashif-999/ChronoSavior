using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCompleteScript : MonoBehaviour
{
    [SerializeField] private GameObject gameCompleteCanvas;

    void Start()
    {
        if (gameCompleteCanvas == null)
        {
            Debug.LogError("GameCompleteCanvas is null.");
        }
        else
        {
            gameCompleteCanvas.SetActive(false);
        }
    }

    public void ShowGameCompleteScreen()
    {
        if (gameCompleteCanvas == null)
        {
            Debug.LogError("GameCompleteCanvas is null.");
            return;
        }

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
