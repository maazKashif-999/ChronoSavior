using UnityEngine;

public class BomberShip : EnemyShip
{
    
    protected override void Start()
    {

        base.Start(); // Call base class (EnemyShip) Start method first
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            limitXPosition = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).x-0.75f;
        }
        
        health = 80;
        MAX_HEALTH = 80;
        speed = 4.0f;
        
        fireInterval = 1f;
        bulletSpeed = 2.0f;
        damage = 25;
        angle = 0;
        coinDroppingProbability = 0.2f;
        powerUpDroppingProbability = 0.5f;
        bulletTag = "BomberBullet";

    }

    protected override void Update()
    {
        base.Update(); // Call base class (EnemyShip) Update method
    }

    protected override void FireBullet()
    {
        base.FireBullet(); // Call base class (EnemyShip) FireBullet method
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other); // Call base class (EnemyShip) OnTriggerEnter2D method
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage); // Call base class (EnemyShip) TakeDamage method
    }
}
