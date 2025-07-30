using System;
using Attacks;
using Damage;
using Health;
using Sensors;
using UnityEngine;

namespace Entities.Enemies._1_TouchEnemy
{
    public class TouchEnemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private EnemySensor chaseSensor;
        [SerializeField] private HealthComponent healthComponent;

        [SerializeField] private Attack attack;

        [SerializeField] private float damage;
        //[SerializeField] private EnemySensor attackSensor;
        
        private Rigidbody2D rb;
        private Player player;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            attack.damage = damage;
        }

        private void OnEnable()
        {
            chaseSensor.OnEnter += OnPlayerEnterChase;

            healthComponent.OnDeath += Die;
        }

        private void OnDisable()
        {
            chaseSensor.OnEnter -= OnPlayerEnterChase;
            
            healthComponent.OnDeath -= Die;
        }

        private void OnPlayerEnterChase(Player player)
        {
            this.player = player;
        }

        private void Update()
        {
            if (!player) return;

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
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}