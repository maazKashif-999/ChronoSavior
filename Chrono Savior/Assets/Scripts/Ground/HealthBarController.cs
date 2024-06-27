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
        int healthIndex = Mathf.FloorToInt(currentHealth / 10f);
        healthIndex = Mathf.Clamp(healthIndex, 0, 9);
        healthBarImage.sprite = healthBarSprites[healthIndex];
    }
}
