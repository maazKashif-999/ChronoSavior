using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public event EventHandler OnWeaponSet;
    [SerializeField] private AmmoUIController ammoUIController; 
    private Camera mainCamera;
    private Vector3 mousePosition;
    private Player player;
    private int selectedWeaponIndex = 0;
    private int[] weaponIndex = new int[] {0, 3, 2};


    // Start is called before the first frame update
    
    void Awake()
    {
        mainCamera = Camera.main;
        player = Player.Instance;
        SelectedWeapon(selectedWeaponIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            return;
        }
        if (player == null || mainCamera == null)
        {
            Debug.LogError("Player or Main Camera not found in WeaponSwitch.");
            return;
        }
        if (!player.IsAlive()) return;

        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePosition - transform.position;
        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        if((rotationZ >= 90 && rotationZ <= 180) || (rotationZ <= -90 && rotationZ >= -180))
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeaponIndex = 0;
            SelectedWeapon(weaponIndex[selectedWeaponIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeaponIndex = 1;
            SelectedWeapon(weaponIndex[selectedWeaponIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeaponIndex = 2;
            SelectedWeapon(weaponIndex[selectedWeaponIndex]);
        }
        else if(Input.GetKeyDown(KeyCode.Tab))
        {
            selectedWeaponIndex = (selectedWeaponIndex + 1) % weaponIndex.Length;
            SelectedWeapon(weaponIndex[selectedWeaponIndex]);
        }
        
    }

    
    void SelectedWeapon(int selectedWeapon)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentWeapon = transform.GetChild(i);
            if (i == selectedWeapon)
            {
                currentWeapon.gameObject.SetActive(true);
                GunShoot gunShoot = currentWeapon.GetComponent<GunShoot>();
                if(gunShoot != null)
                {
                    if(ammoUIController != null)
                    {
                        ammoUIController.SetCurrentGun(gunShoot);
                    }
                    else
                    {
                        Debug.LogError("AmmoUIController not found in WeaponSwitch.");
                    }
                    
                }
                else
                {
                    Debug.LogError("GunShoot component not found in WeaponSwitch.");
                }
                
            }
            else
            {
                currentWeapon.gameObject.SetActive(false);
            }
        }
    }

    public void SetSecondWeapon(int selectedWeapon)
    {
        weaponIndex[1] = selectedWeapon;

        while(weaponIndex[1] == weaponIndex[2])
        {
            weaponIndex[2] = UnityEngine.Random.Range(1,5);
        }
        OnWeaponSet?.Invoke(this,EventArgs.Empty);
    }

    public void SetThirdWeapon(int selectedWeapon)
    {
        weaponIndex[2] = selectedWeapon;
        while(weaponIndex[1] == weaponIndex[2])
        {
            weaponIndex[1] = UnityEngine.Random.Range(1,5);
        }
        OnWeaponSet?.Invoke(this,EventArgs.Empty);
    }

    public string GetSecondWeaponName()
    {
        return transform.GetChild(weaponIndex[1]).name;
    }

    public string GetThirdWeaponName()
    {
        return transform.GetChild(weaponIndex[2]).name;
    }
}
