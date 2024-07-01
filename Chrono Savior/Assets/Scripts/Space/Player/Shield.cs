using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    [SerializeField] private Image shieldImage; 
    [SerializeField] private Sprite[] shieldSprites; 

    public void SetSheild(float amount)
    {
        int index = Mathf.Clamp(Mathf.CeilToInt(amount * (shieldSprites.Length - 1)), 0, shieldSprites.Length - 1);
        if (shieldImage != null)
        {
            shieldImage.sprite = shieldSprites[index];
        }
        else
        {
            Debug.LogWarning("Shield image is not assigned.");
        }
    }
}
