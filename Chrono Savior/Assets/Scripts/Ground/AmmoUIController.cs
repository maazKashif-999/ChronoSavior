using UnityEngine;
using UnityEngine.UI;

public class AmmoUIController : MonoBehaviour
{
    [SerializeField] private Text ammoCountText;

    private GunShoot currentGun;

    // Update is called once per frame
    void Update()
    {
        if (currentGun != null)
        {
            if(currentGun.GetMaxCap() < int.MaxValue)
            {
                ammoCountText.text = $" {currentGun.CurrentCapacity}/{currentGun.CurrentTotalAmmo}";
            }
            else
            {
                ammoCountText.text = $" {currentGun.CurrentCapacity}/inf";
            }
            
        }
        else
        {
            ammoCountText.text = "N/A";
        }
    }

    public void SetCurrentGun(GunShoot gun)
    {
        currentGun = gun;
    }
}
