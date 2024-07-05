using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSessionManager : MonoBehaviour
{
    [SerializeField] private Button loadSessionButton;
    private bool canLoadSession;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(StateManagement.Instance != null)
        {
            canLoadSession = StateManagement.Instance.CanLoadSession();
        }
        if(loadSessionButton != null)
        {
            loadSessionButton.interactable = canLoadSession;
        }
    }

    public void RemoveSession()
    {
        if(StateManagement.Instance != null)
        {
            StateManagement.Instance.SetSessionLoad(false);
        }
        if(loadSessionButton != null)
        {
            loadSessionButton.interactable = false;
        }
    }

}
