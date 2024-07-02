using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float attackDamage = 75f;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private LayerMask playerLayer;
    private SpriteRenderer spriteRenderer;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        Explode();
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
    public void Explode()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, attackRadius,playerLayer);
        foreach(Collider2D player in hitPlayer)
        {
            if(player != null) //is this even necessary? what am i even doing smh help
            {
                Player playerFound = player.GetComponent<Player>();
                if(playerFound != null)
                {
                    playerFound.TakeDamage(attackDamage);
                }
            }
        }
    }

    
}
