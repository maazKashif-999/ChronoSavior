//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Pool;

//public class BulletPoolingAPI : MonoBehaviour
//{
//    public static BulletPoolingAPI SharedInstance;
//    [SerializeField] private List<int> amountsToPool = new List<int>();
//    [SerializeField] private List<BulletScript> bulletsToPool = new List<BulletScript>();
//    private List<ObjectPool<BulletScript>> pooledObjects = new List<ObjectPool<BulletScript>>();

//    // Start is called before the first frame update

//    void Awake()
//    {
//        SharedInstance = this;
//    }
//    void Start()
//    {
//        for(int i = 0; i < bulletsToPool.Count; i++)
//        {
//            int index = i;
//            pooledObjects.Add(new ObjectPool<BulletScript>(()=>CreateBullet(index),null,OnReleaseFromPool,OnBulletDestroy,true,amountsToPool[i],amountsToPool[i]));
//        }
//    }

//    private BulletScript CreateBullet(int index)
//    {
//        BulletScript bullet = Instantiate(bulletsToPool[index]);
//        bullet.gameObject.SetActive(false);
//        return bullet;
//    }

//    // private void OnTakeBulletFromPool(BulletScript bullet)
//    // {
//    //     // bullet.transform.position = bulletTransforms[bulletsToPool.IndexOf(bullet)].transform.position;
//    // }

//    private void OnReleaseFromPool(BulletScript bullet)
//    {
//        bullet.gameObject.SetActive(false);
//    }

//    private void OnBulletDestroy(BulletScript bullet)
//    {
//        Destroy(bullet.gameObject);
//    }

//    public BulletScript GetPooledBullet(int gunIndex)
//    {
//        if (gunIndex >= bulletsToPool.Count || gunIndex < 0) return null;

//        return pooledObjects[gunIndex].Get();
//    }

//    public void ReleaseBullet(BulletScript bullet, int gunIndex)
//    {
//        if (gunIndex >= bulletsToPool.Count || gunIndex < 0) return;
//        if (!bullet.gameObject.activeInHierarchy) return;

//        pooledObjects[gunIndex].Release(bullet);
//    }

//}
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

    void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of BulletPoolingAPI detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (amountsToPool.Count != bulletsToPool.Count)
        {
            Debug.LogError("The counts of amountsToPool and bulletsToPool do not match.");
            return;
        }

        for (int i = 0; i < bulletsToPool.Count; i++)
        {
            if (bulletsToPool[i] == null)
            {
                Debug.LogError($"Bullet prefab at index {i} is null.");
                continue;
            }

            int index = i;
            pooledObjects.Add(new ObjectPool<BulletScript>(() => CreateBullet(index), null, OnReleaseFromPool, OnBulletDestroy, true, amountsToPool[i], amountsToPool[i]));
        }
    }

    private BulletScript CreateBullet(int index)
    {
        BulletScript bullet = Instantiate(bulletsToPool[index]);
        if (bullet != null)
        {
            bullet.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError($"Failed to instantiate bullet at index {index}.");
        }
        return bullet;
    }

    private void OnReleaseFromPool(BulletScript bullet)
    {
        if (bullet != null)
        {
            bullet.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Attempted to release a null bullet.");
        }
    }

    private void OnBulletDestroy(BulletScript bullet)
    {
        if (bullet != null)
        {
            Destroy(bullet.gameObject);
        }
        else
        {
            Debug.LogError("Attempted to destroy a null bullet.");
        }
    }

    public BulletScript GetPooledBullet(int gunIndex)
    {
        if (gunIndex >= bulletsToPool.Count || gunIndex < 0)
        {
            Debug.LogError("Invalid gun index for getting pooled bullet.");
            return null;
        }

        return pooledObjects[gunIndex].Get();
    }

    public void ReleaseBullet(BulletScript bullet, int gunIndex)
    {
        if (gunIndex >= bulletsToPool.Count || gunIndex < 0)
        {
            Debug.LogError("Invalid gun index for releasing bullet.");
            return;
        }

        if (bullet == null)
        {
            Debug.LogError("Attempted to release a null bullet.");
            return;
        }

        if (!bullet.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("Attempted to release a bullet that is not active in the hierarchy.");
            return;
        }

        pooledObjects[gunIndex].Release(bullet);
    }
}
