using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

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
    
    void Start()
    {
        mainCamera = Camera.main;
        player = Player.Instance;
        SelectedWeapon(selectedWeaponIndex);
        if(MainMenu.mode == MainMenu.Mode.Campaign)
        {
            weaponIndex[1] = -1;
            weaponIndex[2] = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            return;
        }
       
        if(player == null)
        {
            Debug.LogError("Player not found in WeaponSwitch.");
            return;
        }
        if(mainCamera == null)
        {
            Debug.LogError("Main Camera not found in WeaponSwitch.");
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
        if(selectedWeapon == -1)
        {
            // for(int i = 0; i < transform.childCount; i++)
            // {
            //     Transform currentWeapon = transform.GetChild(i);
            //     currentWeapon.gameObject.SetActive(false);
            // }
            // if(ammoUIController != null)
            // {
            //     ammoUIController.SetCurrentGun(null);
            // }
            // else
            // {
            //     Debug.LogError("AmmoUIController not found in WeaponSwitch.");
            // }
            selectedWeapon = 0;
        }
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

        // while(weaponIndex[1] == weaponIndex[2])
        // {
        //     weaponIndex[2] = UnityEngine.Random.Range(1,5);
        // }
        if(weaponIndex[1] == weaponIndex[2])
        {
            weaponIndex[2] = -1;
        }
        OnWeaponSet?.Invoke(this,EventArgs.Empty);
    }

    public void SetThirdWeapon(int selectedWeapon)
    {
        weaponIndex[2] = selectedWeapon;
        // while(weaponIndex[1] == weaponIndex[2])
        // {
        //     weaponIndex[1] = UnityEngine.Random.Range(1,5);
        // }
        if(weaponIndex[1] == weaponIndex[2])
        {
            weaponIndex[1] = -1;
        }
        OnWeaponSet?.Invoke(this,EventArgs.Empty);
    }

    public string GetSecondWeaponName()
    {
        if(weaponIndex[1] == -1)
        {
            return "Not Selected";
        }
        return transform.GetChild(weaponIndex[1]).name;
    }

    public string GetThirdWeaponName()
    {
        if(weaponIndex[2] == -1)
        {
            return "Not Selected";
        }
        return transform.GetChild(weaponIndex[2]).name;
    }
}
