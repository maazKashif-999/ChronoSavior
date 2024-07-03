using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class OnPowerupInteract : MonoBehaviour
{
    [SerializeField] private PowerUp powerup;
    [SerializeField] private float messageDuration = 1f; // currently doesnt work for some reason

    private Text messageText;

    private void Start()
    {
        GameObject messageObject = GameObject.FindGameObjectWithTag("PowerUpMessage");
        if (messageObject != null)
        {
            messageText = messageObject.GetComponent<Text>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            powerup.UsePowerUp(other.gameObject);
            string powerUpName = FormatPowerUpName(gameObject.name.Replace("(Clone)", "").Trim());
            ShowMessage(powerUpName + " Activated");
            if(PowerupPoolingAPI.SharedInstance != null)
            {
                // PowerupPoolingAPI.SharedInstance.ReleasePowerup(this, powerup.GameObject.name);
                gameObject.SetActive(false);
                
            }
            else
            {
                Debug.LogError("PowerupPoolingAPI is null in OnPowerupInteract");
            }
        }
    }

    private void ShowMessage(string message)
    {
        if (messageText != null)
        {
            StartCoroutine(DisplayMessage(message));
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
        string formattedName = Regex.Replace(name, "(\\B[A-Z])", " $1");
        return formattedName.Replace("Power Up", "").Trim();
    }
}
