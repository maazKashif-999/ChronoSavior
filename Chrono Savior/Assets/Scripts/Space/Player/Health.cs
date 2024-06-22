using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image health;
    public void SetHealth(float amount)
    {
        health.fillAmount = amount;
    }
}
