using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDeathScript : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        StartCoroutine(RemoveVisibility());
        Destroy(gameObject, 1.5f);
    }

    IEnumerator RemoveVisibility()
    {
        yield return new WaitForSeconds(0.5f);
        if(spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            Debug.LogError("SpriteRenderer is null in TurretDeathScript.");
        }
    }
    
}
