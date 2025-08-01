using System;
using Attacks;
using Damage;
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
        
        private void FixedUpdate()
        {
            rb.velocity = dir * speed;
        }
    }
}