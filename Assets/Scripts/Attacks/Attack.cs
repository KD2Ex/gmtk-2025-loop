using System;
using System.Data.Common;
using Damage;
using UnityEngine;

namespace Attacks
{
    public class Attack : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.1f;
        
        public float damage;
        public float knockbackForce;

        public Action<Collider2D> OnHit;

        private Timer lifeTimer;

        private void Awake()
        {
            lifeTimer = new Timer(lifetime, true);
            lifeTimer.Timeout += Disable;
        }

        private void Update()
        {
            lifeTimer.Tick(Time.deltaTime);
        }

        public void RotateToTarget(Vector2 dir, Transform pivot)
        {
            var angle = Mathf.Atan2(dir.y, dir.x);
            pivot.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle - 90f);
        }

        public void Execute(float damage, float knockbackForce = 0)
        {
            this.damage = damage;
            if (knockbackForce > 0)
            {
                this.knockbackForce = knockbackForce;
            }
            gameObject.SetActive(true);
            lifeTimer.Start();
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            //print(other.gameObject.name);
            var damageable = other.GetComponent<IDamageable>();

            if (damageable == null) return;
            
            var msg = new DamageMessage();
            msg.damage = damage;
            msg.knockbackForce = knockbackForce;
            msg.dir = (other.transform.position - transform.position).normalized;
            damageable.TakeDamage(msg);

            
            OnHit?.Invoke(other);
        }
    }
}