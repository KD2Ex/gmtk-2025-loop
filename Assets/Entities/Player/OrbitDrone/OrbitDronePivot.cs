using System;
using Attacks;
using UnityEngine;

namespace Entities.OrbitDrone
{
    public enum OrbitModifierType
    {
        Damage,
        Radius,
        Heal
    }
    
    public class OrbitDronePivot : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Attack attack;
        [SerializeField] private HealArea healArea;
        [SerializeField] private float radius;
        [SerializeField] private float healAmount = 0f;

        private float damageScale;
        private float baseDamage;
        private Vector3 scale;

        private bool exec;


        public bool IsRunning => exec;

        private void Awake()
        {
            baseDamage = attack.damage;
            scale = attack.transform.localScale;
            UpdateStats();
        }

        private void OnEnable()
        {
            GameManager.instance.OnHubEnter += healArea.Refill;
        }

        private void OnDisable()
        {
            GameManager.instance.OnHubEnter -= healArea.Refill;
        }

        private void Update()
        {
            if (!exec) return;
            
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        }

        public void UpdateStats()
        {
            attack.damage = baseDamage;
            attack.transform.localScale = scale * radius;
            healArea.amount = healAmount;
            
            healArea.gameObject.SetActive(healAmount > 0);
        }

        public void AddModifier(OrbitModifierType type, float value)
        {
            switch (type)
            {
                case OrbitModifierType.Damage:
                    baseDamage += value;
                    break;
                case OrbitModifierType.Radius:
                    radius += value;
                    break;
                case OrbitModifierType.Heal:
                    healAmount += value;
                    break;
            }
            
            UpdateStats();
            
            attack.gameObject.SetActive(true);
            exec = true;
        }
    }
}