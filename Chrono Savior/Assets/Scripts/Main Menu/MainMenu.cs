using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public enum Mode
    {
        Campaign,
        Infinity
    }
    [SerializeField] private GameObject volumePanel;
    public static Mode mode;

    public static bool MidGame = false;
    public static bool EndGame = false;

    private int MainMenuIndex = 0;
    private int Space = 1;
    private int GroundInfinityIndex = 2;
    // private int GroundCampaignIndex = 3;
    private int CutScene = 4;
    //[SerializeField] private CutsceneManager CutsceneManager;

    public void Start()
    {
        if(volumePanel != null)
        {
            volumePanel.SetActive(true);
            volumePanel.SetActive(false);
        }
        
    }
    public void playLevelOne()
    {
        mode = Mode.Campaign;
        MidGame = false;
        SceneManager.LoadScene(CutScene);
    }

    public void playLevelTwo()
    {

        mode = Mode.Campaign;
        MidGame=true;
        SceneManager.LoadScene(CutScene);
    }
    public void playSpaceInfinity()
    {
        mode = Mode.Infinity;
        SceneManager.LoadSceneAsync(Space);

    }

    //public void playLastCutscenes()
    //{
    //    mode = Mode.Campaign;
    //    EndGame = true;
    //    SceneManager.LoadScene(CutScene);
    //}

    public void playGroundInfinity()
    {
        mode = Mode.Infinity;
        SceneManager.LoadSceneAsync(GroundInfinityIndex);
    }

    public void mainMenu()
    {
        SceneManager.LoadSceneAsync(MainMenuIndex);
    }


    public void MuteHandler(bool mute)
    {
        if(mute)
        {
            AudioListener.volume=0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }


}
