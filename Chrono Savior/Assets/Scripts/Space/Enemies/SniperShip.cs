using UnityEngine;

public class SniperShip : EnemyShip
{
    protected override void Start()
    {
        base.Start(); // Call base class (EnemyShip) Start method first
        health = 100;
        speed = 4.0f;
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            limitXPosition = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).x-0.5f;
        }
        fireInterval = 1.5f;
        bulletSpeed = 2.0f;
        MAX_HEALTH = 100;
        damage = 30;
        angle = 270;
        coinDroppingProbability = 0.15f;
        powerUpDroppingProbability = 0.45f;
        bulletTag = "SniperBullet";
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
