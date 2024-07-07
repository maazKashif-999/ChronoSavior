using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float bulletDamage = 10;
    [SerializeField] private float rotation = 90f;
    private GameObject playerCenter;
    private Rigidbody2D rb;
    private const int ENEMY_LAYER = 8;
    private const int POWERUP_LAYER = 11;
    private const int TRIGGER_LAYER = 14;
    private const int ENEMY_BULLET_LAYER = 13;
    private const int PLAYER_BULLET_LAYER = 15;
    private const string PLAYER_CENTER = "PlayerCenter";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCenter = GameObject.FindGameObjectWithTag(PLAYER_CENTER);

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing on EnemyBulletScript GameObject.");
            return;
        }

        if (playerCenter == null)
        {
            Debug.LogError("PlayerCenter not found with tag in EnemyBulletScript.");
            return;
        }

        Vector3 direction = playerCenter.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == ENEMY_LAYER || collision.gameObject.layer == POWERUP_LAYER || collision.gameObject.layer == TRIGGER_LAYER || collision.gameObject.layer == ENEMY_BULLET_LAYER || collision.gameObject.layer == PLAYER_BULLET_LAYER)
        {
            return;
        }

        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(bulletDamage);
            if (gameObject.CompareTag("BomberBullet"))
            {
                player.BossDamage = true;
            }
        }

        Destroy(gameObject);
    }
}
