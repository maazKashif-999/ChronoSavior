using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        Destroy(gameObject, 0.5f);
    }
}
