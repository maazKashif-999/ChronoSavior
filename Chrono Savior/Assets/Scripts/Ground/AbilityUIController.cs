using UnityEngine;
using UnityEngine.UI;

public class AbilityUIController : MonoBehaviour
{
    [SerializeField] private Image abilityImage;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color cooldownColor = Color.grey;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        if (player == null)
        {
            Debug.LogError("Player not found.");
        }

        abilityImage.color = normalColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (player.CanUseAbility())
            {
                abilityImage.color = normalColor;
            }
            else
            {
                abilityImage.color = cooldownColor;
            }
        }
    }
}