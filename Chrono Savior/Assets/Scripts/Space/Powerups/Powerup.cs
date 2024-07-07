//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Powerup : MonoBehaviour
//{
//    [SerializeField] private PowerUpEffect powerUpEffect;
//    [SerializeField] private float messageDuration = 4f;

//    private float screenEdgeX;
//    private Text messageText;

//    private void Start()
//    {
//        screenEdgeX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

//        GameObject messageObject = GameObject.FindGameObjectWithTag("PowerUpMessage1");
//        if (messageObject != null)
//        {
//            messageText = messageObject.GetComponent<Text>();
//        }
//    }

//    void Update()
//    {
//        transform.position += Vector3.left * powerUpEffect.speed * Time.deltaTime;
//        if (transform.position.x < screenEdgeX)
//        {
//            gameObject.SetActive(false);
//        }
//    }

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            powerUpEffect.Apply(other.gameObject);

//            if (PlayerControls.Instance != null && !gameObject.CompareTag("coins") && !gameObject.CompareTag("token"))
//            {
//                PlayerControls.Instance.PlayPowerupSound();
//            }

//            string powerUpName = FormatPowerUpName(gameObject.name.Replace("(Clone)", "").Trim());

//            if (powerUpName != "coins" && powerUpName != "Energy Token")
//            {
//                ShowMessage(powerUpName + " Activated");
//            }

//            gameObject.SetActive(false);
//        }
//    }

//    private void OnDisable()
//    {
//        PoolManager.Instance.ReturnToPool(gameObject.tag, gameObject);
//    }

//    private void OnDestroy()
//    {
//        Debug.Log("Powerup Destroyed");
//    }

//    private void ShowMessage(string message)
//    {
//        if (messageText != null)
//        {
//            CoroutineManager.Instance.StartCor(DisplayMessage(message));
//        }
//    }

//    private IEnumerator DisplayMessage(string message)
//    {
//        messageText.text = message;
//        messageText.enabled = true;
//        yield return new WaitForSeconds(messageDuration);
//        messageText.enabled = false;
//    }

//    private string FormatPowerUpName(string name)
//    {
//        string formattedName = System.Text.RegularExpressions.Regex.Replace(name, "(\\B[A-Z])", " $1");
//        return formattedName.Replace("Power Up", "").Trim();
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Powerup : MonoBehaviour
{
    [SerializeField] private PowerUpEffect powerUpEffect;
    [SerializeField] private float messageDuration = 4f;

    private float screenEdgeX;
    private Text messageText;

    private void Start()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            screenEdgeX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        }
        else
        {
            Debug.LogWarning("Main camera is not assigned.");
        }

        GameObject messageObject = GameObject.FindGameObjectWithTag("PowerUpMessage1");
        if (messageObject != null)
        {
            messageText = messageObject.GetComponent<Text>();
        }
        else
        {
            Debug.LogWarning("Message object with tag 'PowerUpMessage1' not found.");
        }
    }

    void Update()
    {
        if (powerUpEffect != null)
        {
            transform.position += Vector3.left * powerUpEffect.speed * Time.deltaTime;
            if (transform.position.x < screenEdgeX)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("PowerUpEffect is not assigned.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            if (powerUpEffect != null)
            {
                powerUpEffect.Apply(other.gameObject);
            }
            else
            {
                Debug.LogWarning("PowerUpEffect is not assigned.");
            }

            if (PlayerControls.Instance != null && !gameObject.CompareTag("coins") && !gameObject.CompareTag("token"))
            {
                PlayerControls.Instance.PlayPowerupSound();
            }

            string powerUpName = FormatPowerUpName(gameObject.name.Replace("(Clone)", "").Trim());

            if (powerUpName != "coins" && powerUpName != "Energy Token")
            {
                ShowMessage(powerUpName + " Activated");
            }

            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.ReturnToPool(gameObject.tag, gameObject);
        }
        else
        {
            Debug.LogWarning("PoolManager instance is null in OnDisable.");
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Powerup Destroyed");
    }

    private void ShowMessage(string message)
    {
        if (messageText != null)
        {
            if (CoroutineManager.Instance != null)
            {
                CoroutineManager.Instance.StartCor(DisplayMessage(message));
            }
            else
            {
                Debug.LogWarning("CoroutineManager instance is null.");
            }
        }
        else
        {
            Debug.LogWarning("MessageText is not assigned.");
        }
    }

    private IEnumerator DisplayMessage(string message)
    {
        messageText.text = message;
        messageText.enabled = true;
        yield return new WaitForSeconds(messageDuration);
        messageText.enabled = false;
    }

    private string FormatPowerUpName(string name)
    {
        string formattedName = System.Text.RegularExpressions.Regex.Replace(name, "(\\B[A-Z])", " $1");
        return formattedName.Replace("Power Up", "").Trim();
    }
}
