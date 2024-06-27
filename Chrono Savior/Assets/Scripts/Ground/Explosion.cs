using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float attackDamage = 75f;
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
        foreach(Collider2D player in hitPlayer)
        {
            if(player != null) //is this even necessary? what am i even doing smh help
            {
                player.GetComponent<Player>().TakeDamage(attackDamage);
            }
        }
    }

    
}
