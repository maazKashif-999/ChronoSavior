using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public enum Mode
    {
        Campaign,
        Infinity
    }

    public static Mode mode;
    
    private int MainMenuIndex = 0;
    private int Space = 1;
    private int GroundInfinityIndex = 2;
    private int GroundCampaignIndex = 3;
    public void playLevelOne()
    {
        mode = Mode.Campaign;
        SceneManager.LoadSceneAsync(Space);
    }

    public void playLevelTwo()
    {
        mode = Mode.Campaign;
        SceneManager.LoadSceneAsync(GroundCampaignIndex);
    }
    public void playSpaceInfinity()
    {
        mode = Mode.Infinity;
        SceneManager.LoadSceneAsync(Space);

    }
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
