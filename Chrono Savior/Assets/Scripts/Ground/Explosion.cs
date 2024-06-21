using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float attackDamage = 150f;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private LayerMask playerLayer;
    void Start()
    {
        Explode();
        Destroy(gameObject, 0.5f);
    }

    public void Explode()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, attackRadius,playerLayer);
        foreach(Collider2D plyer in hitPlayer)
        {
            plyer.GetComponent<Player>().TakeDamage(attackDamage);
        }
    }

    
}
