using System;
using System.Collections;
using Attacks;
using Damage;
using Knockback;
using Sensors;
using UnityEngine;

namespace Entities.Enemies._4_DashEnemy
{
    public class DashEnemy : Enemy, IDamageable
    {
        [SerializeField] private KnockbackComponent knockback;
        [SerializeField] private Dash dash;
        [SerializeField] private EnemySensor chaseSensor;
        [SerializeField] private EnemySensor attackSensor;
        [SerializeField] private Attack attack;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float damage;
        [SerializeField] private float knockbackForce;
        [SerializeField] private float dashChargeTime;
        [SerializeField] private float dashCooldownTime;
        
        private Player player;
        
        private Timer dashChargeTimer;
        private Timer dashCooldown;

        private bool isPlayerInAttackRange;
        private bool dashCharging;
        private bool isDashReady = true;
        
        private Vector2 lastVelocity;

        protected override void Awake()
        {
            base.Awake();
            ogColor = sprite.color;
            dashChargeTimer = new Timer(dashChargeTime, true);
            dashCooldown = new Timer(dashCooldownTime, true);
        }

        private void Start()
        {
            knockback.velocitySetter = SetVel;
        }

        private void OnEnable()
        {
            chaseSensor.OnEnter += OnChaseSensorEnter;
            attackSensor.OnEnter += OnAttackSensorEnter;
            attackSensor.OnLeave += OnAttackSensorLeave;

            dashChargeTimer.Timeout += OnDashChargeTimeout;
            dashCooldown.Timeout += OnDashCooldown;
            
            dash.Finished += OnDashFinished;
            attack.OnHit += OnAttackHit;
        }

        private void OnDisable()
        {
            chaseSensor.OnEnter -= OnChaseSensorEnter;
            attackSensor.OnEnter -= OnAttackSensorEnter;
            attackSensor.OnLeave -= OnAttackSensorLeave;
            
            dashChargeTimer.Timeout -= OnDashChargeTimeout;
            dashCooldown.Timeout -= OnDashCooldown;
            
            dash.Finished -= OnDashFinished;
            attack.OnHit -= OnAttackHit;
        }

        private void OnChaseSensorEnter(Player obj)
        {
            player = obj;
        }
        
        private void OnAttackSensorEnter(Player obj)
        {
            isPlayerInAttackRange = true;
            // if (dashCharging || dash.IsDashing) return;
            // ChargeDash();
        }
        
        private void OnAttackSensorLeave(Player obj)
        {
            isPlayerInAttackRange = false;
        }

        private void Update()
        {
            
            dashChargeTimer.Tick(Time.deltaTime);
            dashCooldown.Tick(Time.deltaTime);

            if (isPlayerInAttackRange &&
                isDashReady &&
                !dash.IsDashing &&
                !dashCharging)
            {
                ChargeDash();
            }
        }

        private void FixedUpdate()
        {
            if (!player) return;
            if (isPlayerInAttackRange || dashCharging)
            {
                return;
            }
            if (dash.IsDashing)
            {
                return;
            }

            Chase();
            
            if (rb.velocity != Vector2.zero)
            {
                lastVelocity = rb.velocity;
            }
            
            sprite.flipX = lastVelocity.x > 0;
        }

        private void Chase()
        {
            var dir = (player.transform.position - transform.position).normalized;
            
            SetVel(dir, moveSpeed);
            animator.Play("GhostIdle");
        }
        
        private void ChargeDash() 
        {
            dashChargeTimer.Start();
            rb.velocity = Vector2.zero;
            dashCharging = true;
        }

        private void OnDashChargeTimeout()
        {
            var dir = (player.transform.position - transform.position).normalized;
            dash.Execute(dir);
            
            animator.Play("GhostDash");
            
            sprite.flipX = dir.x > 0;
            
            //StartCoroutine(Attacking());
            
            attack.gameObject.SetActive(true);
            
            dashCharging = false;
            
            isDashReady = false;
            dashCooldown.Start();
        }

        private void OnDashCooldown()
        {
            isDashReady = true;
        }

        private IEnumerator Attacking()
        {
            while (dash.TimeRemain < .6)
            {
                yield return null;
            }
            
            attack.Execute(damage, knockbackForce);
        }

        private void OnDashFinished()
        {
            attack.Disable();
        }

        private void OnAttackHit(Collider2D other)
        {
            attack.Disable();
        }

        public void TakeDamage(DamageMessage message)
        {
            health.Remove(message.damage);
            // if (message.knockbackForce > 0)
            // {
            //     knockback.Execute(message.dir, message.knockbackForce);
            // }

            StartCoroutine(Flash());
            // if (health.isDead)
            // {
            //     Die();
            //     return;
            // }

        }
        
        protected override void Die()
        {
            chaseSensor.OnEnter -= OnChaseSensorEnter;
            attackSensor.OnEnter -= OnAttackSensorEnter;
            attackSensor.OnLeave -= OnAttackSensorLeave;
            
            dashChargeTimer.Timeout -= OnDashChargeTimeout;
            dashCooldown.Timeout -= OnDashCooldown;
            
            attack.Disable();
            
            StopAllCoroutines();
            
            sprite.material.SetFloat("_Amount", 0);
            
            
            rb.excludeLayers = LayerMask.GetMask("Player", "Ignore Raycast", "Enemy", "Default");
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            sprite.sortingOrder = -1;
            
            animator.Play("GhostDeath", 0, 0f);
        }

        private void SetVel(Vector2 dir, float force)
        {
            rb.velocity = dir * force;
        }

    }
}