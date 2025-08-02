using System;
using Attacks;
using Damage;
using DoT;
using Entities.DoTEffects;
using UnityEngine;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D rb;
        private Attack hitbox;

        private float speed;
        private Vector2 dir;
        private int penetrateAmount;

        private int penetrated = 0;

        private float dotDamage = 0;
        private float timeBetweenDamage = 0;

        private void Awake()
        {
            hitbox = GetComponent<Attack>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            hitbox.OnHit += OnHit;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //print("Proj hit " + other.gameObject.name);
            if (other.CompareTag("Obstacle")) Destroy(gameObject);
        }

        private void OnHit(Collider2D other)
        {
            //print(other.gameObject.name);
            if (other.CompareTag("Obstacle"))
            {
                Destroy(gameObject);
            }

            if (dotDamage > 0)
            {
                var dotHandler = other.GetComponent<DoTHandler>();
                var dmg = hitbox.damage + hitbox.damage * (dotDamage * 0.01f);
                dotHandler.SetDoTDamage(dmg, timeBetweenDamage);
            }
            
            penetrated++;
            if (penetrated > penetrateAmount)
            {
                Destroy(gameObject);
            }
        }

        public void Init(Vector2 dir, float speed, float damage, float knockbackForce, int penetrateAmount = 0)
        {
            hitbox.damage = damage;
            hitbox.knockbackForce = knockbackForce;
            
            this.speed = speed;
            this.dir = dir;
            this.penetrateAmount = penetrateAmount;
        }

        public void RotateTo(Vector2 dir, float offset = 90)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + offset;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        public void AddDoTEffect(FireDoT dot)
        {
            dotDamage = dot.Damage;
            timeBetweenDamage = dot.TimeBetweenDamage;
        }
        
        private void FixedUpdate()
        {
            rb.velocity = dir * speed;
        }
    }
}