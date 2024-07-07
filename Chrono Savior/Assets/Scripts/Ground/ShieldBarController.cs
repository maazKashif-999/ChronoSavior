using UnityEngine;
using UnityEngine.UI;

public class ShieldBarController : MonoBehaviour
{
    [SerializeField] private Image shieldBarImage;
    [SerializeField] private Sprite[] shieldBarSprites;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        if (player == null)
        {
            Debug.LogError("Player instance is null in ShieldBarController.");
        }

        if (shieldBarImage == null)
        {
            Debug.LogError("ShieldBarImage is not assigned in the inspector.");
        }

        if (shieldBarSprites == null || shieldBarSprites.Length == 0)
        {
            Debug.LogError("ShieldBarSprites array is not assigned or empty in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && shieldBarImage != null && shieldBarSprites != null && shieldBarSprites.Length > 0)
        {
            UpdateShieldBar(player.GetCurrentShield());
        }
    }

    void UpdateShieldBar(float currentShield)
    {
        int shieldIndex = Mathf.FloorToInt(currentShield / (Player.MAX_SHIELD / (float)shieldBarSprites.Length));
        shieldIndex = Mathf.Clamp(shieldIndex, 0, shieldBarSprites.Length - 1);
        shieldBarImage.sprite = shieldBarSprites[shieldIndex];
    }
}
