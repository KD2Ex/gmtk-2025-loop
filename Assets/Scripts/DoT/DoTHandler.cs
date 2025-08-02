using System;
using Entities.Enemies;
using Health;
using UnityEngine;

namespace DoT
{
    public class DoTHandler : MonoBehaviour
    {
        
        public float timeBetweenDamage;
        
        
        private HealthComponent health;
        private Timer damageTimer;
        private Enemy enemy;

        private float damage;

        private void Awake()
        {
            health = GetComponent<HealthComponent>();
            enemy = GetComponent<Enemy>();
            damageTimer = new Timer(timeBetweenDamage, false);
        }

        private void OnEnable()
        {
            damageTimer.Timeout += DealDamage;
        }

        private void OnDisable()
        {
            damageTimer.Timeout -= DealDamage;
        }

        private void Update()
        {
            damageTimer.Tick(Time.deltaTime);
        }

        private void DealDamage()
        {
            health.Remove(damage);
            if (health.isDead)
            {
                enabled = false;
            }
            enemy.ShowDamageText(Mathf.RoundToInt(damage));
            // vfx
        }

        public void SetDoTDamage(float value, float time)
        {
            damage = value;
            timeBetweenDamage = time;


            if (!damageTimer.IsRunning)
            {
                damageTimer.UpdateWaitTime(timeBetweenDamage);
                damageTimer.Start();
            }
        }
    }
}