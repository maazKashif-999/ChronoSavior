using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarController : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Sprite[] healthBarSprites;
    private BossScript boss;

    void Start()
    {
        if (healthBarImage == null)
        {
            Debug.LogError("Health bar image not assigned in the inspector.");
        }

        if (healthBarSprites == null || healthBarSprites.Length == 0)
        {
            Debug.LogError("Health bar sprites not assigned in the inspector.");
        }
    }

    void Update()
    {
        if (boss != null && healthBarImage != null && healthBarSprites.Length > 0)
        {
            UpdateHealthBar(boss.GetCurrentHealth(), boss.GetMaxHealth());
        }
    }

    public void SetBoss(BossScript boss)
    {
        this.boss = boss;
    }

    void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        float healthPercentage = currentHealth / maxHealth;
        int healthIndex = Mathf.FloorToInt(healthPercentage * (healthBarSprites.Length - 1));
        healthIndex = Mathf.Clamp(healthIndex, 0, healthBarSprites.Length - 1);
        healthBarImage.sprite = healthBarSprites[healthIndex];
    }
}
