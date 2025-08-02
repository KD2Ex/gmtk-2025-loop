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
        [SerializeField] private float selfKnockback = 5f;
        //[SerializeField] private EnemySensor attackSensor;
        
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


            attack.OnHit += OnAttackHit;
            afterAttackHit.Timeout += OnAfterAttackHit;
            knockbackTimer.Timeout += OnKnockbackTimerTimeout;
        }

        private void OnDisable()
        {
            chaseSensor.OnEnter -= OnPlayerEnterChase;
            
            
            attack.OnHit -= OnAttackHit;
            afterAttackHit.Timeout -= OnAfterAttackHit;
            knockbackTimer.Timeout -= OnKnockbackTimerTimeout;
        }

        private void OnPlayerEnterChase(Player player)
        {
            this.player = player;
        }

        private void OnAttackHit(Collider2D other)
        {
            disableChase = true;
            rb.velocity = Vector2.zero;
            var dir = (transform.position - player.transform.position).normalized;
            knockbackComponent.Execute(dir, selfKnockback);
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
            //sprite.color = ogColor;
        }

        private void Update()
        {
            if (health.isDead) return;
            
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
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rb.velocity = dir * moveSpeed;
            
            transform.eulerAngles = new Vector3(0, 0, angle + 90);
        }

        public void TakeDamage(DamageMessage message)
        {
            if (health.isDead) return;
            health.Remove(message.damage);

            StartCoroutine(Flash());
            // if (health.isDead)
            // {
            //     Die();
            //     return;
            // }
            
            
            if (message.knockbackForce > 0)
            {
                knockbackComponent.Execute(message.dir, message.knockbackForce);
                knockbacking = true;
                knockbackTimer.Start();
            }
            
            //print("Touch enemey health: " + health.Value);
        }

        protected override void Die()
        {
            animator.Play("SpiderDeath");
            
            rb.excludeLayers = LayerMask.GetMask("Player", "Ignore Raycast", "Enemy", "Default");
            rb.velocity = Vector2.zero;
            
            Destroy(attack);
            Destroy(chaseSensor);
            //Destroy(gameObject);
        }

        private void SetVelocity(Vector2 dir, float force)
        {
            if (health.isDead) return;
            rb.velocity = dir * force;
        }
    }
}