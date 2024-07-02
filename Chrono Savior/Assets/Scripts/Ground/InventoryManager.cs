using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Text secondWeapon;
    [SerializeField] Text thirdWeapon;
    [SerializeField] WeaponSwitch weaponSwitch;
    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 0f;
        PauseMenu.gameIsPaused = true;
    }


    private void Start()
    {
        secondWeapon.text = "Second Weapon: " + weaponSwitch.GetSecondWeaponName();
        thirdWeapon.text = "Third Weapon: " + weaponSwitch.GetThirdWeaponName();
        weaponSwitch.OnWeaponSet += OnWeaponChanged;
        
    }

    private void OnDisable()
    {
        weaponSwitch.OnWeaponSet -= OnWeaponChanged;
    }

    private void OnWeaponChanged(object sender, EventArgs e)
    {
        if(weaponSwitch == null)
        {
            Debug.LogError("WeaponSwitch is null in InventoryManager");
            return;
        }
        secondWeapon.text = "Second Weapon: " + weaponSwitch.GetSecondWeaponName();
        thirdWeapon.text = "Third Weapon: " + weaponSwitch.GetThirdWeaponName();
    
    }

    // Update is called once per frame
    public void Close()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        PauseMenu.gameIsPaused = false;
    }
}
