using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Pool;

public class PowerupPoolingAPI : MonoBehaviour
{
    public static PowerupPoolingAPI SharedInstance;
    [SerializeField] private int amountToPool = 5;
    [SerializeField] private List<OnPowerupInteract> powerupsToPool = new List<OnPowerupInteract>();
    private List<ObjectPool<OnPowerupInteract>> pooledObjects = new List<ObjectPool<OnPowerupInteract>>();

    void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
        }
        else
        {
            Destroy(this);
            Debug.LogError("Multiple instances of PowerupPoolingAPI detected. Destroying duplicate.");
        }
    }

    void Start()
    {
        if (powerupsToPool == null || powerupsToPool.Count == 0)
        {
            Debug.LogError("PowerupsToPool list is not assigned or empty in PowerupPoolingAPI.");
            return;
        }

        for (int i = 0; i < powerupsToPool.Count; i++)
        {
            int index = i;
            if (powerupsToPool[index] != null)
            {
                pooledObjects.Add(new ObjectPool<OnPowerupInteract>(() => CreatePowerup(index), null, OnReleaseFromPool, OnPowerupDestroy, true, amountToPool, amountToPool));
            }
            else
            {
                Debug.LogError($"Powerup at index {index} is null in PowerupPoolingAPI.");
            }
        }
    }

    private OnPowerupInteract CreatePowerup(int index)
    {
        if (index < 0 || index >= powerupsToPool.Count || powerupsToPool[index] == null)
        {
            Debug.LogError($"Invalid powerup index or null powerup in PowerupPoolingAPI: {index}");
            return null;
        }

        OnPowerupInteract powerup = Instantiate(powerupsToPool[index]);
        if(powerup != null)
        {
            powerup.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError($"Failed to instantiate powerup at index {index} in PowerupPoolingAPI.");
        }
        return powerup;
    }

    private void OnReleaseFromPool(OnPowerupInteract powerup)
    {
        if (powerup != null)
        {
            powerup.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Attempted to release a null powerup in PowerupPoolingAPI.");
        }
    }

    private void OnPowerupDestroy(OnPowerupInteract powerup)
    {
        if (powerup != null)
        {
            Destroy(powerup.gameObject);
        }
        else
        {
            Debug.LogError("Attempted to destroy a null powerup in PowerupPoolingAPI.");
        }
    }

    public OnPowerupInteract GetPooledPowerup(int powerupIndex)
    {
        if (powerupIndex >= powerupsToPool.Count || powerupIndex < 0 || pooledObjects[powerupIndex] == null)
        {
            Debug.LogError($"Invalid powerup index or null pooled object in PowerupPoolingAPI: {powerupIndex}");
            return null;
        }

        return pooledObjects[powerupIndex].Get();
    }

    public void ReleasePowerup(OnPowerupInteract powerup, int powerupIndex)
    {
        if (powerupIndex >= powerupsToPool.Count || powerupIndex < 0 || pooledObjects[powerupIndex] == null)
        {
            Debug.LogError($"Invalid powerup index or null pooled object in PowerupPoolingAPI: {powerupIndex}");
            return;
        }

        if (powerup != null)
        {
            pooledObjects[powerupIndex].Release(powerup);
        }
        else
        {
            Debug.LogError("Attempted to release a null powerup in PowerupPoolingAPI.");
        }
    }
}
