using UnityEngine;

public class FighterShip : EnemyShip
{
    protected override void Start()
    {
        base.Start(); // Call base class (EnemyShip) Start method first
        health = 50;
        speed = 4.0f;
        limitXPosition = 5.5f;
        fireInterval = 0.5f;
        bulletSpeed = 4.0f;
        damage = 10;
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
