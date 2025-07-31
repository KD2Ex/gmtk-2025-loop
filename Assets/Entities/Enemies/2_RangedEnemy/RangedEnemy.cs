using System;
using Damage;
using Health;
using Projectiles;
using Sensors;
using UnityEngine;

namespace Entities.Enemies._2_RangedEnemy
{
    public class RangedEnemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemySensor attackSensor;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private float speed = 20;
        [SerializeField] private float damage = 10;
        [SerializeField] private float attackCooldown = 1f;
        
        private Rigidbody2D rb;
        private HealthComponent health;
        private Player player;

        private Timer attackTimer;

        private void Awake()
        {
            attackTimer = new Timer(attackCooldown, false);
            health = GetComponent<HealthComponent>();
        }

        private void Update()
        {
            attackTimer.Tick(Time.deltaTime);
        }

        private void OnEnable()
        {
            attackSensor.OnEnter += OnPlayerEnterAttackSensor;
            attackSensor.OnLeave += OnPlayerLeaveAttackSensor;
            
            attackTimer.Timeout += OnAttackTimerTimeout;

            health.OnDeath += () => Destroy(gameObject);
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
            print("Player leave");
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
            health.Remove(message.damage);
        }
    }
}