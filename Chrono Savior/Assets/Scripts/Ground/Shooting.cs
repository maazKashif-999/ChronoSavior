using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private AmmoUIController ammoUIController; 

    private Vector3 mousePosition;
    private int selectedWeaponIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectedWeapon(selectedWeaponIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Player.Instance.IsAlive()) return;

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
            SelectedWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectedWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectedWeapon(2);
        }
    }

    void SelectedWeapon(int selectedWeapon)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == selectedWeapon)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                ammoUIController.SetCurrentGun(transform.GetChild(i).GetComponent<GunShoot>());
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
