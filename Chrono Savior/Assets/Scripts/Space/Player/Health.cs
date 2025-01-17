using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthImage;
    [SerializeField] private Sprite[] healthSprites;

    public void SetHealth(float amount)
    {
        if (healthSprites == null || healthSprites.Length == 0)
        {
            Debug.LogWarning("Health sprites are not assigned or empty.");
            return;
        }

        int index = Mathf.Clamp(Mathf.CeilToInt(amount * (healthSprites.Length - 1)), 0, healthSprites.Length - 1);

        if (healthImage != null)
        {
            healthImage.sprite = healthSprites[index];
        }
        else
        {
            Debug.LogWarning("Health image is not assigned.");
        }
    }
}
