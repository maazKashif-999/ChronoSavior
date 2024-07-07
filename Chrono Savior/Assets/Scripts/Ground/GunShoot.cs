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
    private const int PISTOL_INDEX = 0;
    private const int AR_INDEX = 1;
    private const int SNIPER_INDEX = 2;
    private const int SMG_INDEX = 3;
    private const int SHOTGUN_INDEX = 4;
    [SerializeField] private AudioSource myaudio;
    [SerializeField] private AudioClip bulletSound;
    [SerializeField] private AudioClip reloadSound;

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
        if (currentCapacity <= 0 || (Input.GetKeyDown(KeyCode.R) && currentCapacity < maxCapacity))
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
            if (bulletTransform == null)
            {
                Debug.LogError("Bullet Transform is null in GunShoot.");
                return;
            }

            BulletScript bullet = null;
            if (Player.Instance == null)
            {
                Debug.Log("Player is null");
                return;
            }
            if (!Player.Instance.IsAlive() || PauseMenu.gameIsPaused) return;

            if (BulletPoolingAPI.SharedInstance != null)
            {
                if (gunType == PISTOL)
                {
                    bullet = BulletPoolingAPI.SharedInstance.GetPooledBullet(PISTOL_INDEX);
                }
                else if (gunType == AR)
                {
                    bullet = BulletPoolingAPI.SharedInstance.GetPooledBullet(AR_INDEX);
                }
                else if (gunType == SNIPER)
                {
                    bullet = BulletPoolingAPI.SharedInstance.GetPooledBullet(SNIPER_INDEX);
                }
                else if (gunType == SMG)
                {
                    bullet = BulletPoolingAPI.SharedInstance.GetPooledBullet(SMG_INDEX);
                }
                else if (gunType == SHOTGUN)
                {
                    bullet = BulletPoolingAPI.SharedInstance.GetPooledBullet(SHOTGUN_INDEX);
                }
            }

            if (bullet != null)
            {
                bullet.transform.position = bulletTransform.position;
                bullet.transform.rotation = Quaternion.identity;
                bullet.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Bullet Pool not found in GunShoot.");
            }

            if (bullet != null && myaudio != null && bulletSound != null)
            {
                myaudio.PlayOneShot(bulletSound);
            }
            else
            {
                Debug.LogWarning("Audio source or explosion sound clip is not assigned.");
            }
            currentTotalAmmo--;
            currentCapacity--;
        }
    }

    IEnumerator Reload()
    {
        if (myaudio != null && reloadSound != null)
        {
            myaudio.PlayOneShot(reloadSound);
        }

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
