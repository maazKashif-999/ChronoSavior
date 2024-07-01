using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool SharedInstance;

    [SerializeField] private List<int> amountsToPool = new List<int>();
    [SerializeField] private List<GameObject> objectsToPool = new List<GameObject>();
    [SerializeField] private List<List<GameObject>> pooledObjects = new List<List<GameObject>>();

    void Awake()
    {
        SharedInstance = this;
    }
    // Start is called before the first frame update
    
    void Start()
    {
        for(int i = 0; i < objectsToPool.Count; i++)
        {
            pooledObjects.Add(new List<GameObject>());
            GameObject tmp;
            for(int j = 0; j < amountsToPool[i]; j++)
            {
                tmp = Instantiate(objectsToPool[i]);
                tmp.SetActive(false);
                pooledObjects[i].Add(tmp);
            }
        }
    }

    public GameObject GetPooledBullet(int gunIndex)
    {
        if(gunIndex >= objectsToPool.Count || gunIndex < 0) return null;
        
        for(int i = 0; i < amountsToPool[gunIndex]; i++)
        {
            if(!pooledObjects[gunIndex][i].activeInHierarchy)
            {
                return pooledObjects[gunIndex][i];
            }
        }
        
        return null;
    }


}
