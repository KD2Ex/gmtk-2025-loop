using System;
using Attacks;
using Damage;
using UnityEngine;

namespace Entities.OrbitDrone
{
    public class OrbitDrone: MonoBehaviour
    {
        public Attack attack;

        public float radius = 1f;

        private void Update()
        {
            
        }

        public void UpdateStats()
        {
            radius = 1f;
            attack.transform.localScale *= radius;
        }

        // public float Damage = 50;
        // public float KnockbackForce = 10;
        //
        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     var damageable = other.GetComponent<IDamageable>();
        //
        //     if (damageable == null) return;
        //     
        //     var dir = (other.transform.position - transform.position).normalized;
        //
        //     var msg = new DamageMessage
        //     {
        //         damage = Damage,
        //         knockbackForce = KnockbackForce,
        //         dir = dir
        //     };
        //     
        //     damageable.TakeDamage(msg);
        // }
    }
}