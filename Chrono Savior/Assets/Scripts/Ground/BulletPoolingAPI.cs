using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPoolingAPI : MonoBehaviour
{
    public static BulletPoolingAPI SharedInstance;
    [SerializeField] private List<int> amountsToPool = new List<int>();
    [SerializeField] private List<BulletScript> bulletsToPool = new List<BulletScript>();
    private List<ObjectPool<BulletScript>> pooledObjects = new List<ObjectPool<BulletScript>>();
    
    // Start is called before the first frame update
    
    void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {
        for(int i = 0; i < bulletsToPool.Count; i++)
        {
            int index = i;
            pooledObjects.Add(new ObjectPool<BulletScript>(()=>CreateBullet(index),null,OnReleaseFromPool,OnBulletDestroy,true,amountsToPool[i],amountsToPool[i]));
        }
    }

    private BulletScript CreateBullet(int index)
    {
        BulletScript bullet = Instantiate(bulletsToPool[index]);
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    // private void OnTakeBulletFromPool(BulletScript bullet)
    // {
    //     // bullet.transform.position = bulletTransforms[bulletsToPool.IndexOf(bullet)].transform.position;
    // }

    private void OnReleaseFromPool(BulletScript bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnBulletDestroy(BulletScript bullet)
    {
        Destroy(bullet.gameObject);
    }

    public BulletScript GetPooledBullet(int gunIndex)
    {
        if (gunIndex >= bulletsToPool.Count || gunIndex < 0) return null;

        return pooledObjects[gunIndex].Get();
    }

    public void ReleaseBullet(BulletScript bullet, int gunIndex)
    {
        if (gunIndex >= bulletsToPool.Count || gunIndex < 0) return;

        pooledObjects[gunIndex].Release(bullet);
    }
    
}
