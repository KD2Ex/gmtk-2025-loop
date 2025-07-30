using System;
using Damage;
using UnityEngine;

namespace Attacks
{
    public class Attack : MonoBehaviour
    {
        public float damage;

        public void Execute(float damage)
        {
            this.damage = damage;
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var damageable = other.GetComponent<IDamageable>();
            var msg = new DamageMessage();
            msg.damage = damage;
            damageable.TakeDamage(msg);
        }
    }
}