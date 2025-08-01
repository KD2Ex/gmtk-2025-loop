using System;
using Health;
using UnityEngine;

namespace DoT
{
    public class DoTHandler : MonoBehaviour
    {
        
        public float timeBetweenDamage;
        
        
        private HealthComponent health;
        private Timer damageTimer;

        private float damage;

        private void Awake()
        {
            health = GetComponent<HealthComponent>();
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