using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image health;
    public void SetHealth(float amount)
    {
        if (health != null)
        {
            health.fillAmount = amount;
        }
        else
        {
            Debug.LogWarning("Health image is not assigned.");
        }
    }
   

}