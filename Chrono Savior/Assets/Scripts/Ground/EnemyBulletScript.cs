using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    [SerializeField] private float force;
    [SerializeField] private float bulletDamage = 10;
    [SerializeField] private float rotation = 90f;
    private int enemyLayer = 8;
    private int powerupLayer = 11;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("PlayerCenter");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x,direction.y).normalized * force;

        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rot + rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == enemyLayer || collision.gameObject.layer == powerupLayer) return;
        Player player = collision.GetComponent<Player>();
        if(player)
        {
            player.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
