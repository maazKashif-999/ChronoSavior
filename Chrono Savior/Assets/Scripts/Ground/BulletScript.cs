using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePosition;
    private Camera mainCamera;
    private Rigidbody2D rb;
    [SerializeField] private float force;
    [SerializeField] private float bulletDamage;
    private int playerLayer = 6;
    private int powerupLayer = 11;
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        rb.velocity = new Vector2(direction.x,direction.y).normalized * force;
        float rot = (Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg) + 90;
        transform.rotation = Quaternion.Euler(0,0,rot);
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == playerLayer || collision.gameObject.layer == powerupLayer) return;
        IEnemy enemy = collision.GetComponent<IEnemy>();
        if(enemy != null)
        {
            float currentDamage = bulletDamage * Player.Instance.GetDamageMultipler();
            enemy.TakeDamage(currentDamage);
        }
        Destroy(gameObject);
    }
}
 