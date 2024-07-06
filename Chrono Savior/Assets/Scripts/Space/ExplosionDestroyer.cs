using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void dest()
    {
        gameObject.SetActive(false);
        PoolManager.Instance.ReturnToPool(gameObject.tag, gameObject);
    }
}
