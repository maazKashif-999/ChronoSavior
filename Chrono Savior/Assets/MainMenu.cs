using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void playLevelOne()
    {
        // GameManager.Instance.SetGMState(GameManager.GameManagerState.playing);
        SceneManager.LoadSceneAsync(1);
    }

    public void playSpaceInfinity()
    {
        // GameManager.Instance.SetGMState(GameManager.GameManagerState.);
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
}