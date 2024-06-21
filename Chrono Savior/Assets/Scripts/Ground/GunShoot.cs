using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    private float timer;
    public float timeBeteenShots;
     
    public int maxCapacity;
    private int currentCapacity;
    public float reloadTime = 1f;
    private bool isReloading = false;
    [SerializeField] private int MAX_AMMO_CAP = int.MaxValue;
    private int currentTotalAmmo;
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
        if(isReloading)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            isReloading = true;
            StartCoroutine(Reload());
            return;
        }
        if(currentCapacity <= 0)
        {
            isReloading = true;
            StartCoroutine(Reload());
            return;
        }
        if(!canFire)
        {
            timer += Time.deltaTime;
            if(timer >= timeBeteenShots)
            {
                canFire = true;
                timer = 0;
            }
        }
        if(Input.GetMouseButton(0) && canFire && currentTotalAmmo > 0)
        {
            canFire = false;
            Instantiate(bullet,bulletTransform.position,Quaternion.identity);
            currentTotalAmmo--;
            currentCapacity--;
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        if(currentTotalAmmo >= maxCapacity)
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
}
