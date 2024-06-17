using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float speed = 5f; // Speed of the player's movement
    float minY; // Minimum Y position
    float maxY; // Maximum Y position

    public GameObject explosion;
    int health = 100;

    float bulletSpeed = 6.0f;

    int damage = 10;
    public GameObject playerBulletPrefab; // Prefab of the player bullet

    void Start()
    {
        // Calculate minY and maxY based on the camera's viewport
        minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        maxY = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)).y;

        // Set the starting position
        transform.position = new Vector3(-6, 0, 0);

        // Set the starting rotation
        transform.rotation = Quaternion.Euler(0, 0, 270);
    }

    void Update()
    {
        // Fire bullet when user clicks left mouse button
        if (Input.GetMouseButtonDown(0))
        {
           FireBullet();
        }

        Move();
    }

    void Move()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 newPosition = transform.position + new Vector3(0, verticalInput * speed * Time.deltaTime, 0);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = newPosition;
    }

    void FireBullet()
    {
            GameObject bulletObject = Instantiate(playerBulletPrefab, transform.position, Quaternion.identity);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; 
            PlayerBullet bulletScript = bulletObject.GetComponent<PlayerBullet>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(mousePosition,bulletSpeed,damage);
            }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullets"))
        {
            Debug.Log("Player got hit!");
            PlayerBullet bullet = other.GetComponent<PlayerBullet>();

            if (bullet != null)
            {
                Debug.Log(bullet.damage);
                TakeDamage(bullet.damage); 
                Destroy(other.gameObject); 
            }
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            explode();
            Debug.Log("Player Dead");
            Destroy(gameObject);
        }
    }

    void explode()
    {
        GameObject PlayerExposion = (GameObject)Instantiate(explosion);
        PlayerExposion.transform.position = transform.position;
    }

}
