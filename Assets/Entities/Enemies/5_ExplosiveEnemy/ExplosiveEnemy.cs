using System;
using System.Collections;
using System.Collections.Generic;
using Attacks;
using Damage;
using Entities.Enemies;
using Health;
using Sensors;
using UnityEngine;

public class ExplosiveEnemy : Enemy, IDamageable
{
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private EnemySensor chaseSensor;
    [SerializeField] private EnemySensor attackSensor;
    [SerializeField] private CircleCollider2D hitbox;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accel = 15;
    [SerializeField] private float accelTime;


    private Player player;

    private float elapsed = 0f;


    private void OnEnable()
    {
        chaseSensor.OnEnter += OnPlayerSpotted;
        attackSensor.OnEnter += OnReachedPlayer;
    }
    
    private void OnDisable()
    {
        chaseSensor.OnEnter -= OnPlayerSpotted;
        attackSensor.OnEnter -= OnReachedPlayer;
    }

    private void FixedUpdate()
    {
        if (!player) return;
        if (health.isDead) return;

        elapsed += Time.deltaTime;
        var curveValue = movementCurve.Evaluate(elapsed / accelTime);
        var dir = (player.transform.position - transform.position).normalized;
        var vel = dir * (moveSpeed * curveValue);

        moveSpeed += accel * Time.deltaTime;
        
        rb.velocity = vel;
    }

    private void OnPlayerSpotted(Player player)
    {
        this.player = player;
    }

    private void OnReachedPlayer(Player player)
    {
        health.isDead = true;
        Detonate();
        Die();
        
        attackSensor.OnEnter -= OnReachedPlayer;
    }

    public void TakeDamage(DamageMessage message)
    {
        health.Remove(message.damage);
        if (health.isDead)
        {
            Die();
        }
    }

    protected override void Die()
    {
        rb.velocity = Vector2.zero;
        //rb.excludeLayers = LayerMask.GetMask("Enemy", "Default");
        hitbox.excludeLayers = LayerMask.GetMask("Enemy", "Ignore Raycast", "Player", "Default");
        //sprite.color = Color.black;
        animator.Play("TimeDeath");
        //Destroy(gameObject);
    }

    private void Detonate()
    {
        attack.Execute(damage, knockbackForce);
    }
}

