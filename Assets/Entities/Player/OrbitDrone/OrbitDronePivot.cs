using System;
using Attacks;
using Health;
using UnityEngine;
using UnityEngine.Serialization;

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
        [FormerlySerializedAs("healAmount")] [SerializeField] private float healAmountScale = 0f;

        private float damageScale = 0.25f;
        private Vector3 scale;
        private Quaternion ogRot;

        private float rotationSpeedScale = 1;
        private float ogRotationSpeed;


        
        private bool exec;

        private float ogDamageScale;


        private HealthComponent playerHealth;
        public bool IsRunning => exec;

        private void Awake()
        {
            ogDamageScale = damageScale;
            ogRotationSpeed = rotationSpeed;
            
            scale = attack.transform.localScale;
            ogRot = transform.rotation;
        }

        private void OnEnable()
        {
            GameManager.instance.OnHubEnter += healArea.Refill;
            GameManager.instance.Player.Health.OnValueChanged += UpdateStats;
        }

        private void OnDisable()
        {
            GameManager.instance.OnHubEnter -= healArea.Refill;
            GameManager.instance.Player.Health.OnValueChanged -= UpdateStats;
        }

        private void Start()
        {
            playerHealth = GameManager.instance.Player.Health;
            UpdateStats(0, playerHealth.MaxValue);
        }

        private void Update()
        {
            if (!exec) return;
            
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        }

        public void UpdateStats(float _, float maxHealth)
        {
            var dmg = maxHealth * damageScale;
            attack.damage = dmg;
            
            attack.transform.localScale = scale * radius;
            healArea.amount = playerHealth.MaxValue * healAmountScale;
            
            healArea.gameObject.SetActive(healAmountScale > 0);

            rotationSpeed = ogRotationSpeed * rotationSpeedScale;
        }

        public void AddModifier(OrbitModifierType type, float value)
        {
            switch (type)
            {
                case OrbitModifierType.Damage:
                    damageScale += value;
                    break;
                case OrbitModifierType.Radius:
                    radius += value;
                    break;
                case OrbitModifierType.Heal:
                    healAmountScale += value;
                    break;
            }

            rotationSpeedScale += 0.1f;
            
            UpdateStats(playerHealth.Value, playerHealth.MaxValue);
            
            attack.gameObject.SetActive(true);
            exec = true;
        }

        public void ResetOrbit()
        {
            radius = 1;
            rotationSpeedScale = 1;
            rotationSpeed = ogRotationSpeed;
            healAmountScale = 0;
            exec = false;
            attack.gameObject.SetActive(false);
            attack.damage = 100f * ogDamageScale; // 100 default health * default damage scale

            transform.rotation = ogRot;
        }
    }
}