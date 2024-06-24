using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void playLevelOne()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void playLevelTwo()
    {
        SceneManager.LoadSceneAsync(2);
    }


}