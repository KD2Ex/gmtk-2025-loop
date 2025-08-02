using System;
using Damage;
using Health;
using Projectiles;
using Sensors;
using UnityEngine;

namespace Entities.Enemies._2_RangedEnemy
{
    public class RangedEnemy : Enemy, IDamageable
    {
        [SerializeField] private EnemySensor attackSensor;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private float speed = 20;
        [SerializeField] private float damage = 10;
        [SerializeField] private float attackCooldown = 1f;
        
        private Player player;

        private Timer attackTimer;

        protected override void Awake()
        {
            base.Awake();
            attackTimer = new Timer(attackCooldown, false);
        }

        private void Update()
        {
            if (health.isDead) return;
            attackTimer.Tick(Time.deltaTime);
        }

        private void OnEnable()
        {
            attackSensor.OnEnter += OnPlayerEnterAttackSensor;
            attackSensor.OnLeave += OnPlayerLeaveAttackSensor;
            
            attackTimer.Timeout += OnAttackTimerTimeout;

        }
        
        private void OnDisable()
        {
            attackSensor.OnEnter -= OnPlayerEnterAttackSensor;
            attackSensor.OnLeave -= OnPlayerLeaveAttackSensor;
            
            attackTimer.Timeout -= OnAttackTimerTimeout;
        }
        
        private void OnPlayerEnterAttackSensor(Player player)
        {
            this.player = player;
            //Shoot();
            attackTimer.Start();
        }

        private void OnPlayerLeaveAttackSensor(Player player)
        {
            //print("Player leave");
            attackTimer.Pause();
        }

        private void OnAttackTimerTimeout()
        {
            if (!player) return;
            Shoot();
        }

        public void Shoot()
        {
            var inst = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            
            var dir = (player.transform.position - transform.position).normalized;
            
            inst.Init(dir, speed, damage, 0);
        }

        public void TakeDamage(DamageMessage message)
        {
            if (health.isDead) return;
            
            health.Remove(message.damage);

            if (health.isDead)
            {
                Die();
                return;
            }
            
            StartCoroutine(Flash());
        }

        private void Die()
        {
            animator.Play("EEDeath");
            attackTimer.Stop();
            rb.excludeLayers = LayerMask.GetMask("Player", "Ignore Raycast", "Enemy", "Default");
            
            attackSensor.OnEnter -= OnPlayerEnterAttackSensor;
            attackSensor.OnLeave -= OnPlayerLeaveAttackSensor;
        }
    }
}