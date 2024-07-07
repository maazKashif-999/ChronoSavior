using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager _instance;

    public static CoroutineManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("CoroutineManager");
                _instance = obj.AddComponent<CoroutineManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.LogWarning("Multiple instances of CoroutineManager detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    public void StartCor(IEnumerator coroutine)
    {
        if (coroutine == null)
        {
            Debug.LogError("Attempted to start a null coroutine.");
            return;
        }

        StartCoroutine(coroutine);
    }
}
