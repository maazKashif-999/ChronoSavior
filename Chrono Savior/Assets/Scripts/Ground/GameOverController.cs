using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverCanvas;

    void Start()
    {
        if (gameOverCanvas == null)
        {
            Debug.LogError("GameOverCanvas is null.");
        }
        else
        {
            gameOverCanvas.SetActive(false);
        }
    }

    public void ShowGameOverScreen()
    {
        if (gameOverCanvas == null)
        {
            Debug.LogError("GameOverCanvas is null.");
            return;
        }

        gameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }
}
