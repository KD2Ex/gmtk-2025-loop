using System;
using System.Collections;
using System.Data.Common;
using Damage;
using Unity.VisualScripting;
using UnityEngine;

namespace Attacks
{
    public class Attack : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.1f;
        [SerializeField] private float collisionLifetime = 0.1f;

        private Collider2D coll;
        
        public float damage;
        public float knockbackForce;

        public Action<Collider2D> OnHit;

        private Timer lifeTimer;
        private Timer collisionLifeTimer;
        

        private void Awake()
        {
            coll = GetComponent<Collider2D>();
            
            lifeTimer = new Timer(lifetime, true);
            lifeTimer.Timeout += Disable;
            
            collisionLifeTimer = new Timer(lifetime, true);
            collisionLifeTimer.Timeout += DisableCollision;
        }

        private void Update()
        {
            lifeTimer.Tick(Time.deltaTime);
            collisionLifeTimer.Tick(Time.deltaTime);
        }

        public void RotateToTarget(Vector2 dir, Transform pivot)
        {
            var angle = Mathf.Atan2(dir.y, dir.x);
            pivot.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle - 90f);
        }

        public void Execute(float damage, float knockbackForce = 0, bool disableObject = true)
        {
            this.damage = damage;
            if (knockbackForce > 0)
            {
                this.knockbackForce = knockbackForce;
            }

            // if (gameObject.activeInHierarchy)
            // {
            //     StartCoroutine(Spam());
            // }
            // else
            // {
            //     gameObject.SetActive(true);
            //     coll.enabled = true;
            // }
            if (disableObject)
            {
                gameObject.SetActive(true);
                lifeTimer.Start();
            }
            else
            {
                if (coll.enabled)
                {
                    StartCoroutine(Spam());
                    return;
                }
                coll.enabled = true;
                collisionLifeTimer.Start();
            }
        }

        private IEnumerator Spam()
        {
            coll.enabled = false;
            yield return null;
            coll.enabled = true;
            collisionLifeTimer.Start();
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        private void DisableCollision()
        {
            coll.enabled = false;
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