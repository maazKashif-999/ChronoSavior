using UnityEngine;

public class BomberShip : EnemyShip
{
    protected override void Start()
    {
        base.Start(); // Call base class (EnemyShip) Start method first
        health = 80;
        speed = 4.0f;
        limitXPosition = 5.5f;
        fireInterval = 1f;
        bulletSpeed = 2.0f;
        damage = 25;
        angle = 180;
        coinDroppingProbability = 0.2f;
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
