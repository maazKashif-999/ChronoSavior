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
        SharedInstance = this;
    }
    void Start()
    {
        pooledCoins = new ObjectPool<CoinScript>(CreateCoin,null,OnReleaseFromPool,OnCoinDestroy,true,amountToPool,amountToPool);
    }

    private CoinScript CreateCoin()
    {
        CoinScript coin = Instantiate(coinPrefab);
        coin.gameObject.SetActive(false);
        return coin;
    }

    private void OnReleaseFromPool(CoinScript coin)
    {
        coin.gameObject.SetActive(false);
    }

    private void OnCoinDestroy(CoinScript coin)
    {
        Destroy(coin.gameObject);
    }

    public CoinScript GetPooledCoin()
    {
        return pooledCoins.Get();
    }

    public void Release(CoinScript coin)
    {
        if (!coin.gameObject.activeInHierarchy) return;
        pooledCoins.Release(coin);
    }
}
