using System;
using System.Collections;
using Attacks;
using Damage;
using Health;
using Sensors;
using UnityEngine;

namespace Entities.Enemies._3_TankEnemy
{
    public class TankEnemy : Enemy, IDamageable
    {
        [SerializeField] private EnemySensor chaseSensor;
        [SerializeField] private EnemySensor attackSensor;
        [SerializeField] private Attack attack;
        [SerializeField] private Transform attackPivot;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float damage;
        [SerializeField] private float knockbackForce;
        [SerializeField] private float attackDelayTime = .4f;
        [SerializeField] private float attackRestoreTime = .4f;
        [SerializeField] private float attackCooldownTime = 1f;

        private Player player;

        private bool playerInAttackRange = false;
        private bool blockMovement = false;
        private bool attackCharging = false;

        private Timer attackDelay;
        private Timer attackRestore;
        private Timer attackCooldown;

        protected override void Awake()
        {
            base.Awake();
            
            attackDelay = new Timer(attackDelayTime, true);
            attackRestore = new Timer(attackDelayTime, true);
            attackCooldown = new Timer(attackCooldownTime, true);

            attack.damage = damage;
            attack.knockbackForce = knockbackForce;
        }

        private void Update()
        {
            attackDelay.Tick(Time.deltaTime);
            attackRestore.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            
            if (!player) return;
            if (playerInAttackRange || blockMovement || attackCharging)
            {
                rb.velocity = Vector2.zero;
                return;
            }
            
            Chase();
        }

        private void OnEnable()
        {
            chaseSensor.OnEnter += OnChaseSensorEnter;
            attackSensor.OnEnter += OnAttackSensorEnter;
            attackSensor.OnLeave += OnAttackSensorLeave;

            attackDelay.Timeout += OnAttackDelayTimeout;
            attackRestore.Timeout += OnAttackRestoreTimeout;
        }

        private void OnDisable()
        {
            chaseSensor.OnEnter -= OnChaseSensorEnter;
            attackSensor.OnEnter -= OnAttackSensorEnter;
            attackSensor.OnLeave -= OnAttackSensorLeave;
            
            attackDelay.Timeout -= OnAttackDelayTimeout;
            attackRestore.Timeout -= OnAttackRestoreTimeout;
        }

        private void OnChaseSensorEnter(Player player)
        {
            this.player = player;
        }

        private void OnAttackSensorEnter(Player player)
        {
            if (attackCharging) return;
            //print("Attack sensor entered");
            playerInAttackRange = true;
            
            attackDelay.Start(); // start attack animation
            attackCharging = true;
            //var dir = (player.transform.position - transform.position).normalized;
            //attack.RotateToTarget(dir, attackPivot);
            
            //attack.Execute(damage);
            blockMovement = true;
            rb.velocity = Vector2.zero;
        }
        
        private void OnAttackSensorLeave(Player player)
        {
            playerInAttackRange = false;
        }

        private void OnAttackDelayTimeout()
        {
            Attack();
            attackCharging = false;
        }

        private void OnAttackRestoreTimeout()
        {
            //
            if (playerInAttackRange)
            {
                Attack();
            }
            else
            {
                blockMovement = false;
            }
        }

        private void Attack()
        {
            attack.Execute(damage);
            attackRestore.Start();
        }
        
        private void Chase()
        {
            var dir = (player.transform.position - transform.position).normalized;
            rb.velocity = dir * moveSpeed;
        }

        public void TakeDamage(DamageMessage message)
        {
            health.Remove(message.damage);
            if (health.isDead)
            {
                Die();
                return;
            }
            sprite.color = Color.white;
            
            StopAllCoroutines();
            StartCoroutine(Flash());
        }


        private void Die()
        {
            Destroy(gameObject);
        }
    }
}