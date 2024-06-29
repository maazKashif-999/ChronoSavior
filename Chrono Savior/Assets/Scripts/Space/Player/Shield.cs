using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    [SerializeField] private Image shield;
    public void SetSheild(float amount)
    {
        if (shield != null)
        {
            shield.fillAmount = amount;
        }
        else
        {
            Debug.LogWarning("Shield image is not assigned.");
        }
    }

}