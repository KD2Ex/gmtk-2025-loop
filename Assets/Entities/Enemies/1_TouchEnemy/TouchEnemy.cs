using System;
using Attacks;
using Damage;
using Health;
using Sensors;
using UnityEngine;

namespace Entities.Enemies._1_TouchEnemy
{
    public class TouchEnemy : Enemy, IDamageable
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private EnemySensor chaseSensor;
        [SerializeField] private HealthComponent healthComponent;

        [SerializeField] private Attack attack;

        [SerializeField] private float damage;
        [SerializeField] private float knockbackForce;
        //[SerializeField] private EnemySensor attackSensor;
        
        private Rigidbody2D rb;
        private Player player;

        private bool disableChase = false;

        private Timer afterAttackHit;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            attack.damage = damage;
            attack.knockbackForce = knockbackForce;

            afterAttackHit = new Timer(.5f, true);
        }

        private void OnEnable()
        {
            chaseSensor.OnEnter += OnPlayerEnterChase;

            healthComponent.OnDeath += Die;

            attack.OnHit += OnAttackHit;
            afterAttackHit.Timeout += OnAfterAttackHit;
        }

        private void OnDisable()
        {
            chaseSensor.OnEnter -= OnPlayerEnterChase;
            
            healthComponent.OnDeath -= Die;
            
            attack.OnHit -= OnAttackHit;
            afterAttackHit.Timeout -= OnAfterAttackHit;
        }

        private void OnPlayerEnterChase(Player player)
        {
            this.player = player;
        }

        private void OnAttackHit(int layer)
        {
            disableChase = true;
            rb.velocity = Vector2.zero;
            afterAttackHit.Start();
        }

        private void OnAfterAttackHit()
        {
            disableChase = false;
        }

        private void Update()
        {
            afterAttackHit.Tick(Time.deltaTime);
            
            if (!player) return;
            if (disableChase) return;

            Chase();
        }

        private void Chase()
        {
            var dir = (player.transform.position - transform.position).normalized;
            rb.velocity = dir * moveSpeed;
        }

        public void TakeDamage(DamageMessage message)
        {
            
            healthComponent.Remove(message.damage);
            print("Touch enemey health: " + healthComponent.Value);
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}