using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    private InventoryManager inventoryManager;
    private CampaignInventoryManager campaignInventoryManager;
    

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        campaignInventoryManager = FindObjectOfType<CampaignInventoryManager>();
        if (pauseMenuUI == null)
        {
            Debug.LogError("PauseMenuUI is not assigned!");
            return;
        }

        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(inventoryManager != null)
            {
                if(inventoryManager.gameObject.activeInHierarchy)
                {
                    HandleEscapeOnMenu();
                }
                else
                {
                    HandleEscape();
                
                }
            }
            else if(campaignInventoryManager != null)
            {
                if(campaignInventoryManager.gameObject.activeInHierarchy)
                {
                    HandleEscapeOnMenu();
                }
                else
                {
                    HandleEscape();
                
                }
            }
            else
            {
                HandleEscape();
            }
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            if((inventoryManager != null && inventoryManager.gameObject.activeInHierarchy) || (campaignInventoryManager != null && campaignInventoryManager.gameObject.activeInHierarchy))
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
                gameIsPaused = false;
            }
            
            pauseMenuUI.SetActive(false);
            // Time.timeScale = 1f;
            // gameIsPaused = false;
        }
    }

    void Pause()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            gameIsPaused = true;
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        SceneManager.LoadScene("Main Menu"); 
    }

    private void HandleEscape()
    {
        if (gameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void HandleEscapeOnMenu()
    {
        if(pauseMenuUI != null)
        {
            if(!pauseMenuUI.activeInHierarchy)
            {
                pauseMenuUI.SetActive(true);
            }
            else
            {
                pauseMenuUI.SetActive(false);
            }
        }
    }
}
