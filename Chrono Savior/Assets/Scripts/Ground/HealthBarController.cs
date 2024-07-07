using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Sprite[] healthBarSprites;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        if (player == null)
        {
            Debug.LogError("Player instance not found in HealthBarController.");
        }

        if (healthBarImage == null)
        {
            Debug.LogError("HealthBarImage is not assigned in HealthBarController.");
        }

        if (healthBarSprites == null || healthBarSprites.Length == 0)
        {
            Debug.LogError("HealthBarSprites are not assigned or empty in HealthBarController.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            UpdateHealthBar(player.GetCurrentHealth());
        }
        else
        {
            Debug.LogError("Player not found in HealthBarController.");
        }
    }

    void UpdateHealthBar(float currentHealth)
    {
        if (healthBarImage == null || healthBarSprites == null || healthBarSprites.Length == 0)
        {
            Debug.LogError("HealthBarImage or HealthBarSprites not properly assigned in UpdateHealthBar.");
            return;
        }

        int healthIndex = Mathf.FloorToInt(currentHealth / 10f);
        healthIndex = Mathf.Clamp(healthIndex, 0, 9);
        healthBarImage.sprite = healthBarSprites[healthIndex];
    }
}
