using System.Collections;
using System.Collections.Generic;
using Health;
using UnityEngine;

namespace Entities.Enemies
{
    public class Enemy : MonoBehaviour
    {
        protected HealthComponent health;
        protected SpriteRenderer sprite;
        protected Rigidbody2D rb;
        protected Color ogColor;

        protected virtual void Awake()
        {
            health = GetComponent<HealthComponent>();
            rb = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            ogColor = sprite.color;
        }
        protected IEnumerator Flash()
        {
            yield return new WaitForSeconds(0.3f);
            sprite.color = ogColor;
        }
    }
}