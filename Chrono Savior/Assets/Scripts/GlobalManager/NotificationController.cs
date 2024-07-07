using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour
{
    public Text messageText; // Reference to the text component where the message will be displayed

    // Method to set the message text of the notification
    public void SetMessage(string message)
    {

        if (messageText != null)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = message;
        }
    }
}
