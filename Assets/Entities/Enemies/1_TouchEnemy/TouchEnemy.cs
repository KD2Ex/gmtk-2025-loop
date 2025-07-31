using System;
using Attacks;
using Damage;
using Health;
using Knockback;
using Sensors;
using UnityEngine;

namespace Entities.Enemies._1_TouchEnemy
{
    public class TouchEnemy : Enemy, IDamageable
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private EnemySensor chaseSensor;
        [SerializeField] private KnockbackComponent knockbackComponent;

        [SerializeField] private Attack attack;

        [SerializeField] private float damage;
        [SerializeField] private float knockbackForce;
        //[SerializeField] private EnemySensor attackSensor;
        
        private Color ogColor;
        private Player player;

        private bool disableChase = false;

        private Timer afterAttackHit;
        private Timer knockbackTimer;
        
        private bool knockbacking = false;

        protected override void Awake()
        {
            base.Awake();
            ogColor = sprite.color;

            attack.damage = damage;
            attack.knockbackForce = knockbackForce;

            afterAttackHit = new Timer(.5f, true);
            knockbackTimer = new Timer(.3f, true);

            knockbackComponent.velocitySetter = SetVelocity;
        }

        private void OnEnable()
        {
            chaseSensor.OnEnter += OnPlayerEnterChase;

            health.OnDeath += Die;

            attack.OnHit += OnAttackHit;
            afterAttackHit.Timeout += OnAfterAttackHit;
            knockbackTimer.Timeout += OnKnockbackTimerTimeout;
        }

        private void OnDisable()
        {
            chaseSensor.OnEnter -= OnPlayerEnterChase;
            
            health.OnDeath -= Die;
            
            attack.OnHit -= OnAttackHit;
            afterAttackHit.Timeout -= OnAfterAttackHit;
            knockbackTimer.Timeout -= OnKnockbackTimerTimeout;
        }

        private void OnPlayerEnterChase(Player player)
        {
            this.player = player;
        }

        private void OnAttackHit(int layer)
        {
            disableChase = true;
            rb.velocity = Vector2.zero;
            var dir = (transform.position - player.transform.position).normalized;
            knockbackComponent.Execute(dir, 15);
            afterAttackHit.Start();
        }

        private void OnAfterAttackHit()
        {
            disableChase = false;
        }

        private void OnKnockbackTimerTimeout()
        {
            knockbacking = false;
            rb.velocity = Vector2.zero;
            sprite.color = ogColor;
        }

        private void Update()
        {
            afterAttackHit.Tick(Time.deltaTime);
            knockbackTimer.Tick(Time.deltaTime);
            
            if (!player) return;
            if (disableChase) return;
            if (knockbacking) return;

            Chase();
        }

        private void Chase()
        {
            var dir = (player.transform.position - transform.position).normalized;
            rb.velocity = dir * moveSpeed;
        }

        public void TakeDamage(DamageMessage message)
        {
            health.Remove(message.damage);
            sprite.color = Color.white;
            
            knockbackComponent.Execute(message.dir, 7.5f);
            knockbacking = true;
            knockbackTimer.Start();
            
            print("Touch enemey health: " + health.Value);
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        private void SetVelocity(Vector2 dir, float force)
        {
            rb.velocity = dir * force;
        }
    }
}