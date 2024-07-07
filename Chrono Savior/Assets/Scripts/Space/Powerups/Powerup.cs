using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField] private PowerUpEffect powerUpEffect;
    private float screenEdgeX;



    private void Start()
    {
        screenEdgeX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

    }
    void Update()
    {
        transform.position += Vector3.left * powerUpEffect.speed * Time.deltaTime;
        if (transform.position.x < screenEdgeX)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            powerUpEffect.Apply(other.gameObject);

            if(PlayerControls.Instance!=null && !gameObject.CompareTag("coins") && !gameObject.CompareTag("token"))
            {
                PlayerControls.Instance.PlayPowerupSound();
            }
            gameObject.SetActive(false);
        }
    }
    private void OnDisable() {
        PoolManager.Instance.ReturnToPool(gameObject.tag, gameObject);
    }
    private void OnDestroy() {
        Debug.Log("Powerup Destroyed");
    }

    private void setActiveFunc()
     {
           gameObject.SetActive(false);

     }

}
