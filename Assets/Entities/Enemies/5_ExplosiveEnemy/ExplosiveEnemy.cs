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
    [SerializeField] private Attack attack;
    [SerializeField] private EnemySensor chaseSensor;
    [SerializeField] private EnemySensor attackSensor;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelTime;
    [SerializeField] private float damage;
    [SerializeField] private float knockbackForce;


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
        rb.velocity = vel;
    }

    private void OnPlayerSpotted(Player player)
    {
        this.player = player;
    }

    private void OnReachedPlayer(Player player)
    {
        health.isDead = true;
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

    private void Die()
    {
        attack.Execute(damage, knockbackForce);
        rb.velocity = Vector2.zero;
        sprite.color = Color.black;
        //Destroy(gameObject);
    }
}

