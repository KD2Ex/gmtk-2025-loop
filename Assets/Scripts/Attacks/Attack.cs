using System;
using System.Data.Common;
using Damage;
using UnityEngine;

namespace Attacks
{
    public class Attack : MonoBehaviour
    {
        public float damage;
        public float knockbackForce;

        public Action<int> OnHit;

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
            print(other.gameObject.name);
            var damageable = other.GetComponent<IDamageable>();

            if (damageable == null) return;
            
            var msg = new DamageMessage();
            msg.damage = damage;
            msg.knockbackForce = knockbackForce;
            msg.dir = (other.transform.position - transform.position).normalized;
            damageable.TakeDamage(msg);

            
            OnHit?.Invoke(other.gameObject.layer);
        }
    }
}