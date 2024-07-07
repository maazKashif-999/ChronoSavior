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
        screenEdgeX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

        GameObject messageObject = GameObject.FindGameObjectWithTag("PowerUpMessage1");
        if (messageObject != null)
        {
            messageText = messageObject.GetComponent<Text>();
        }
    }

    void Update()
    {
        transform.position += Vector3.left * powerUpEffect.speed * Time.deltaTime;
        if (transform.position.x < screenEdgeX)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            powerUpEffect.Apply(other.gameObject);

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
        PoolManager.Instance.ReturnToPool(gameObject.tag, gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("Powerup Destroyed");
    }

    private void ShowMessage(string message)
    {
        if (messageText != null)
        {
            CoroutineManager.Instance.StartCor(DisplayMessage(message));
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
