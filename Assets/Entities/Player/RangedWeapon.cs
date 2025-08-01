using System;
using Entities.DoTEffects;
using Projectiles;
using UnityEngine;

namespace Entities
{
    public class RangedWeapon : MonoBehaviour
    {
        [SerializeField] private Projectile projPrefab;
        [SerializeField] private float speed;
        [SerializeField] private float damage;
        [SerializeField] private float knockbackForce;
        [SerializeField] private int maxAmmo;
        [SerializeField] private int consumePerShot;
        [SerializeField] public float generatePerHit;
        [SerializeField] private float cooldown = .2f;

        private Timer cooldownTimer;
        private bool isReady = true;

        private int currentAmmo;
        private float generationProgress;

        public Action<int> OnAmmoChanged;

        public float OgAmmoGen;
        public float OgDamage => damage;
        public float OgCooldown => cooldown;
        public float TotalDamage; 
        public float TotalCooldown;

        public FireDoT firDot;
        

        private void Awake()
        {
            OgAmmoGen = generatePerHit;
            cooldownTimer = new Timer(cooldown, true);
            currentAmmo = maxAmmo;
            
            TotalDamage = damage; 
            TotalCooldown = cooldown; 
        }

        private void OnEnable()
        {
            cooldownTimer.Timeout += OnCooldown;
        }

        private void OnDisable()
        {
            cooldownTimer.Timeout -= OnCooldown;
        }

        private void OnCooldown()
        {
            isReady = true;
        }

        private void Update()
        {
            cooldownTimer.Tick(Time.deltaTime);
        }

        public void Shoot(Vector2 dir)
        {
            if (!isReady) return;
            if (currentAmmo <= 0) return;
            
            var inst = Instantiate(projPrefab, transform.position, Quaternion.identity);
            inst.Init(dir, speed, TotalDamage, knockbackForce);
            inst.AddDoTEffect(firDot);
            
            cooldownTimer.UpdateWaitTime(TotalCooldown);
            cooldownTimer.Start();
            isReady = false;

            currentAmmo -= consumePerShot;
            
            OnAmmoChanged?.Invoke(currentAmmo);
        }

        public void GenerateAmmo()
        {
            if (currentAmmo == maxAmmo) return;
            generationProgress += generatePerHit;
            if (generationProgress >= 1f)
            {
                var amount = (int)generationProgress;
                currentAmmo += amount;
                generationProgress -= amount;
                
                currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
                
                OnAmmoChanged?.Invoke(currentAmmo);
            }
        }
    }
}