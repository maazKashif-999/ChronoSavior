using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PowerupPoolingAPI : MonoBehaviour
{
    public static PowerupPoolingAPI SharedInstance;
    [SerializeField] private int amountToPool = 5;
    [SerializeField] private List<OnPowerupInteract> powerupsToPool = new List<OnPowerupInteract>();
    private List<ObjectPool<OnPowerupInteract>> pooledObjects = new List<ObjectPool<OnPowerupInteract>>();
    
    // Start is called before the first frame update
    
    void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {
        for(int i = 0; i < powerupsToPool.Count; i++)
        {
            int index = i;
            pooledObjects.Add(new ObjectPool<OnPowerupInteract>(()=>CreatePowerup(index),null,OnReleaseFromPool,OnPowerupDestroy,true,amountToPool,amountToPool));
        }
    }

    private OnPowerupInteract CreatePowerup(int index)
    {
        OnPowerupInteract powerup = Instantiate(powerupsToPool[index]);
        powerup.gameObject.SetActive(false);
        return powerup;
    }

    private void OnReleaseFromPool(OnPowerupInteract powerup)
    {
        powerup.gameObject.SetActive(false);
    }

    private void OnPowerupDestroy(OnPowerupInteract powerup)
    {
        Destroy(powerup.gameObject);
    }

    public OnPowerupInteract GetPooledPowerup(int powerupIndex)
    {
        if (powerupIndex >= powerupsToPool.Count || powerupIndex < 0) return null;

        return pooledObjects[powerupIndex].Get();
    }

    public void ReleasePowerup(OnPowerupInteract powerup, int powerupIndex)
    {
        if (powerupIndex >= powerupsToPool.Count || powerupIndex < 0) return;

        pooledObjects[powerupIndex].Release(powerup);
    }
    
}
