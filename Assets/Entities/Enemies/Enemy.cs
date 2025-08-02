using System.Collections;
using System.Collections.Generic;
using Health;
using UnityEngine;

namespace Entities.Enemies
{
    public class Enemy : MonoBehaviour
    {
        protected HealthComponent health;
        public SpriteRenderer sprite;
        public Animator animator;
        protected Rigidbody2D rb;
        protected Color ogColor;

        [SerializeField] private GameObject coinPrefab;

        protected virtual void Awake()
        {
            health = GetComponent<HealthComponent>();
            rb = GetComponent<Rigidbody2D>();
            //sprite = GetComponent<SpriteRenderer>();
            ogColor = sprite.color;
            health.OnDeath += DropGold;
            health.OnDeath += Die;
            
        }
        protected IEnumerator Flash()
        {
            sprite.material.SetFloat("_Amount", 1);
            yield return new WaitForSeconds(0.3f);
            sprite.material.SetFloat("_Amount", 0);
        }

        protected virtual void DropGold()
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }

        protected virtual void Die()
        {
        }
    }
}