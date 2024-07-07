//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerBullet : MonoBehaviour
//{
//    private Vector3 targetPosition;
//    private Vector3 direction;
//    private float speed;
//    private int damage;

//    public int Damage
//    {
//        get { return damage; }
//    }

//    public void Initialize(Vector3 target, float bulletSpeed, int bulletDamage, Quaternion rotation)
//    {
//        targetPosition = target;
//        speed = bulletSpeed;
//        damage = bulletDamage;
//        direction = (targetPosition - transform.position).normalized;

//        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle))*rotation;

//    }

//    void Update()
//    {
//        transform.Translate(direction * speed * Time.deltaTime, Space.World);
//        if (Camera.main != null)
//        {
//            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
//            if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height)
//            {
//                gameObject.SetActive(false);
//            }
//        }
//        else
//        {
//            Debug.LogWarning("Main camera is not assigned.");
//        }
//    }


//    void OnDisable()
//    {
//        PoolManager.Instance.ReturnToPool(gameObject.tag, gameObject);
//    }
//    void OnDestroy()
//    {
//        Debug.Log("Bullet Destroyed");
//    }


//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 direction;
    private float speed;
    private int damage;

    public int Damage
    {
        get { return damage; }
    }

    public void Initialize(Vector3 target, float bulletSpeed, int bulletDamage, Quaternion rotation)
    {
        targetPosition = target;
        speed = bulletSpeed;
        damage = bulletDamage;
        direction = (targetPosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)) * rotation;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        if (Camera.main != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("Main camera is not assigned.");
        }
    }

    void OnDisable()
    {
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.ReturnToPool(gameObject.tag, gameObject);
        }
        else
        {
            Debug.LogWarning("PoolManager instance is null in OnDisable.");
        }
    }

    void OnDestroy()
    {
        Debug.Log("Bullet Destroyed");
    }
}
