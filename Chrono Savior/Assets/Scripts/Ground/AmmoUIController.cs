using UnityEngine;
using UnityEngine.UI;

public class AmmoUIController : MonoBehaviour
{
    [SerializeField] private Text ammoCountText;
    private GunShoot currentGun;

    // Update is called once per frame
    void Update()
    {
        if (ammoCountText != null)
        {
            if (currentGun != null)
            {
                if (currentGun.GetMaxCap() < int.MaxValue)
                {
                    ammoCountText.text = $" {currentGun.CurrentCapacity}/{currentGun.CurrentTotalAmmo}";
                }
                else
                {
                    ammoCountText.text = $" {currentGun.CurrentCapacity}/MAX";
                }
            }
            else
            {
                ammoCountText.text = "N/A";
            }
        }
        else
        {
            Debug.LogError("Ammo count text UI element not assigned.");
        }
    }

    public void SetCurrentGun(GunShoot gun)
    {
        if (gun == null)
        {
            Debug.LogWarning("Attempted to set null gun.");
            return;
        }
        currentGun = gun;
    }
}
