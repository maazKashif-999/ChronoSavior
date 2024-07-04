using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private string bulletType;
    [SerializeField] private float force;
    [SerializeField] private float bulletDamage;
    private float[] multipler = {1.0f,1.1f,1.2f,1.3f,1.4f,1.5f};
    private int multiplerIndex = 0;
    private Vector3 mousePosition;
    private Camera mainCamera;
    private Rigidbody2D rb;
    private const int PLAYER_LAYER = 6;
    private const int POWERUP_LAYER = 11;
    private const string AR = "AR";
    private const string SMG = "SMG";
    private const string PISTOL = "Pistol";
    private const string SHOTGUN = "Shotgun";
    private const string SNIPER = "Sniper";
    private const int PISTOL_INDEX = 0;
    private const int AR_INDEX = 1;
    private const int SNIPER_INDEX = 2;
    private const int SMG_INDEX = 3;
    private const int SHOTGUN_INDEX = 4;
    
    void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        //is this fine as a null check
       
    }

    void Start()
    {
        if(StateManagement.Instance != null)
        {
            multiplerIndex = StateManagement.Instance.GetUpgradeIndex(bulletType);
            if(bulletType == SMG)
            {
                Debug.Log(multiplerIndex);
            }
            
        }
        else
        {
            Debug.LogError("Player is null in BulletScript");
        }
    }
    void OnEnable()
    {
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
            float currentDamage = bulletDamage * Player.Instance.GetDamageMultipler() * multipler[multiplerIndex];
            enemy.TakeDamage(currentDamage);
        }
        
        // gameObject.SetActive(false);
        if (!gameObject.activeInHierarchy) return;
        if(BulletPoolingAPI.SharedInstance != null)
        {
            if(bulletType == PISTOL)
            {
                BulletPoolingAPI.SharedInstance.ReleaseBullet(this,PISTOL_INDEX);
            }
            else if(bulletType == AR)
            {
                BulletPoolingAPI.SharedInstance.ReleaseBullet(this,AR_INDEX);
            }
            else if(bulletType == SNIPER)
            {
                BulletPoolingAPI.SharedInstance.ReleaseBullet(this,SNIPER_INDEX);
            }
            else if(bulletType == SMG)
            {
                BulletPoolingAPI.SharedInstance.ReleaseBullet(this,SMG_INDEX);
            }
            else if(bulletType == SHOTGUN)
            {
                BulletPoolingAPI.SharedInstance.ReleaseBullet(this,SHOTGUN_INDEX);
            }
        }
        else
        {
            Debug.LogError("BulletPool is null in BulletScript");
        }
        
    }
}
 