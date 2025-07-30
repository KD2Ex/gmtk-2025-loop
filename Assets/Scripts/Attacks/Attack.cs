using System;
using UnityEngine;

namespace Attacks
{
    public class Attack : MonoBehaviour
    {
        public float damage;

        public void Execute(float damage)
        {
            
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            //var damageable = other.GetComponent
            //var msg = new DamageMessage();
            //damageble.TakeDamage(msg);
        }
    }
}