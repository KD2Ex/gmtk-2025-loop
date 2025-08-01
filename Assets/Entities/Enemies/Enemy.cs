using System.Collections;
using System.Collections.Generic;
using Attacks;
using Health;
using TMPro;
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
        [SerializeField] protected Attack attack;
        [SerializeField] protected float damage;
        [SerializeField] protected float knockbackForce;

        protected float scale;

        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private GameObject textPrefab;

        protected virtual void Awake()
        {
            scale = GameManager.instance.GetEnemyScale();
            
            health = GetComponent<HealthComponent>();
            rb = GetComponent<Rigidbody2D>();
            //sprite = GetComponent<SpriteRenderer>();
            ogColor = sprite.color;
            health.OnDeath += DropGold;
            health.OnDeath += Die;
            
            var hp = health.Value * scale;
            
            health.Value = hp;
            health.MaxValue = hp;

            
            UpdateDamage();

            var text = GetComponentInChildren<HPText>();
            if (text)
                text.gameObject.SetActive(GameManager.instance.EnableEnemyHP);

        }

        protected virtual void Start()
        {
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

        public void ShowDamageText(int damageMessage)
        {
            Vector3 pointToSpawn = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            GameObject textMessage = Instantiate(textPrefab, pointToSpawn, Quaternion.identity);

            textMessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = damageMessage.ToString();
            textMessage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = damageMessage.ToString();
            Destroy(textMessage, 0.5f);
        }
        
        protected virtual void UpdateDamage()
        {
            damage *= scale;
            print(damage);
            print(GameManager.instance.DifficultyLevel);
            print(scale);
        }
    }
}