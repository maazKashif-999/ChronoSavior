using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playButton;
    public GameObject player;
    public GameObject enemies;
    public enum GameManagerState{
        opening,
        playing,
        gameover
    }
    GameManagerState GMState;
    // Start is called before the first frame update
    void Start()
    {
        GMState = GameManagerState.opening;
    }

    void UpdateGMState()
    {
        switch(GMState){
            case GameManagerState.opening:
            player.SetActive(false);
            enemies.SetActive(false);
            break;
            case GameManagerState.playing:
            playButton.SetActive(false);
            player.GetComponent<PlayerControls>().Init();
            enemies.SetActive(true);
            break;
            case GameManagerState.gameover:
            player.SetActive(false);
            enemies.SetActive(false);
            break;
        }
    }

    void setGMState(GameManagerState state)
    {
        GMState = state;
        UpdateGMState();
    }


    public void startGame()
    {
        GMState = GameManagerState.playing;
        UpdateGMState();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
