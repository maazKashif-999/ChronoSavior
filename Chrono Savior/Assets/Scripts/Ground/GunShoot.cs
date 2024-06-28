using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    [SerializeField] private string gunType;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletTransform;
    [SerializeField] private bool canFire;
    [SerializeField] private float timeBeteenShots;
    [SerializeField] private int maxCapacity;
    [SerializeField] private float reloadTime = 1f;
    [SerializeField] private int MAX_AMMO_CAP = int.MaxValue;
    private float timer;
    public int MaxCapacity => maxCapacity; // Add this line
    private int currentCapacity;
    public int CurrentCapacity => currentCapacity; // Add this line
    private bool isReloading = false;
    private int currentTotalAmmo;
    public int CurrentTotalAmmo => currentTotalAmmo; // Add this line
    private const string AR = "AR";
    private const string SMG = "SMG";
    private const string PISTOL = "Pistol";
    private const string SHOTGUN = "Shotgun";
    private const string SNIPER = "Sniper";

    // Update is called once per frame
    void Start()
    {
        currentCapacity = maxCapacity;
        currentTotalAmmo = MAX_AMMO_CAP;
    }

    void OnEnable()
    {
        isReloading = false;
    }

    void Update()
    {
        if (isReloading)
        {
            return;
        }
        if (currentCapacity <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            isReloading = true;
            StartCoroutine(Reload());
            return;
        }
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer >= timeBeteenShots)
            {
                canFire = true;
                timer = 0;
            }
        }
        if (Input.GetMouseButton(0) && canFire && currentTotalAmmo > 0)
        {
            canFire = false;
            if(bulletTransform == null)
            {
                Debug.LogError("Bullet Transform is null in GunShoot.");
                return;
            }
            GameObject bullet = null;
            if(gunType == AR)
            {
                bullet = ARBulletPool.SharedInstance.GetPooledObject(); 
            }
            else if(gunType == SMG)
            {
                bullet = SMGBulletPool.SharedInstance.GetPooledObject(); 
            }
            else if(gunType == PISTOL)
            {
                bullet = PistolBulletPool.SharedInstance.GetPooledObject(); 
            }
            else if(gunType == SNIPER)
            {
                bullet = SniperBulletPool.SharedInstance.GetPooledObject(); 
            }
            // else if(gunType == SHOTGUN)
            // {
            //     bullet = ShotgunBulletPool.SharedInstance.GetPooledObject(); 
            // }
            if (bullet != null) 
            {
                bullet.transform.position = bulletTransform.position;
                bullet.transform.rotation = Quaternion.identity;
                bullet.SetActive(true);
            }
            currentTotalAmmo--;
            currentCapacity--;
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        if (currentTotalAmmo >= maxCapacity)
        {
            currentCapacity = maxCapacity;
        }
        else
        {
            currentCapacity = currentTotalAmmo;
        }
        isReloading = false;
    }

    public void ReplenishAmmo()
    {
        currentCapacity = maxCapacity;
        currentTotalAmmo = MAX_AMMO_CAP;
    }

    public int GetMaxCap()
    {
        return MAX_AMMO_CAP;
    }
}
