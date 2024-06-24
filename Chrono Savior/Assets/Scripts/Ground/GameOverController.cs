using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverCanvas;

    // Start is called before the first frame update
    void Start()
    {
        if (gameOverCanvas == null)
        {
            Debug.LogError("GameOverCanvas not assigned in the inspector.");
        }
        else
        {
            gameOverCanvas.SetActive(false);
        }
    }

    public void ShowGameOverScreen()
    {
        Debug.Log("ShowGameOverScreen called"); 
        gameOverCanvas.SetActive(true); 
        Time.timeScale = 0f; 
    }

    public void PlayAgain()
    {
        Debug.Log("Play Again called");
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadSceneAsync(0);
    }
    
    // private void OnDisable()
    // {
    //     Time.timeScale = 1f;
    // }
}

