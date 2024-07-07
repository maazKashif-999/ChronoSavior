using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CoinPoolingAPI : MonoBehaviour
{
    public static CoinPoolingAPI SharedInstance;
    [SerializeField] private int amountToPool = 30;
    [SerializeField] private CoinScript coinPrefab;
    private ObjectPool<CoinScript> pooledCoins;

    void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of CoinPoolingAPI detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (coinPrefab == null)
        {
            Debug.LogError("CoinPrefab is not assigned in CoinPoolingAPI.");
            return;
        }

        pooledCoins = new ObjectPool<CoinScript>(CreateCoin, null, OnReleaseFromPool, OnCoinDestroy, true, amountToPool, amountToPool);
    }

    private CoinScript CreateCoin()
    {
        CoinScript coin = Instantiate(coinPrefab);
        if (coin != null)
        {
            coin.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Failed to instantiate coin.");
        }
        return coin;
    }

    private void OnReleaseFromPool(CoinScript coin)
    {
        if (coin != null)
        {
            coin.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Attempted to release a null coin.");
        }
    }

    private void OnCoinDestroy(CoinScript coin)
    {
        if (coin != null)
        {
            Destroy(coin.gameObject);
        }
        else
        {
            Debug.LogError("Attempted to destroy a null coin.");
        }
    }

    public CoinScript GetPooledCoin()
    {
        if (pooledCoins == null)
        {
            Debug.LogError("PooledCoins object pool is not initialized.");
            return null;
        }

        return pooledCoins.Get();
    }

    public void Release(CoinScript coin)
    {
        if (coin == null)
        {
            Debug.LogError("Attempted to release a null coin.");
            return;
        }

        if (!coin.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("Attempted to release a coin that is not active in the hierarchy.");
            return;
        }

        pooledCoins.Release(coin);
    }
}
