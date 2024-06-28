using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private string bulletType;
    [SerializeField] private float force;
    [SerializeField] private float bulletDamage;
    private Vector3 mousePosition;
    private Camera mainCamera;
    private Rigidbody2D rb;
    private const int PLAYER_LAYER = 6;
    private const int POWERUP_LAYER = 11;
    void OnEnable()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        //is this fine as a null check?
        if(mainCamera != null && rb != null)
        {
            mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - transform.position;
            rb.velocity = new Vector2(direction.x,direction.y).normalized * force;
            float rot = (Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg) + 90;
            transform.rotation = Quaternion.Euler(0,0,rot);
        }
        else
        {
            Debug.LogError("Camera or Rigidbody2D is null in Bulletscript");
        }
       
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == PLAYER_LAYER || collision.gameObject.layer == POWERUP_LAYER) return;
        IEnemy enemy = collision.GetComponent<IEnemy>();
        if(enemy != null)
        {
            float currentDamage = bulletDamage * Player.Instance.GetDamageMultipler();
            enemy.TakeDamage(currentDamage);
        }
        
        gameObject.SetActive(false);
        
    }
}
 