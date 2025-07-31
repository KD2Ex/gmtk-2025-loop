using System;
using Projectiles;
using Sensors;
using UnityEngine;

namespace Entities.Enemies._2_RangedEnemy
{
    public class RangedEnemy : MonoBehaviour
    {
        [SerializeField] private EnemySensor attackSensor;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private float speed = 20;
        [SerializeField] private float damage = 10;
        [SerializeField] private float attackCooldown = 1f;
        
        private Rigidbody2D rb;

        private Player player;

        private Timer attackTimer;

        private void Awake()
        {
            attackTimer = new Timer(attackCooldown, false);
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
    }
}