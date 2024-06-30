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

    public void playLevelOne()
    {
        mode = Mode.Campaign;
        SceneManager.LoadSceneAsync(1);
    }

    public void playSpaceInfinity()
    {
        mode = Mode.Infinity;
        SceneManager.LoadSceneAsync(1);

    }

    public void playLevelTwo()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void mainMenu()
    {
        SceneManager.LoadSceneAsync(0);
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
