using System;
using Attacks;
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

        [SerializeField] private Attack explosvieAttackPrefab;

        public float ExplosiveDamage = 0;
        public float ExplosiveRadiusScale = 1;

        private Timer cooldownTimer;
        private bool isReady = true;

        public bool IsReady => isReady;

        private int currentAmmo;
        private float generationProgress;

        public Action<int> OnAmmoChanged;

        public float OgAmmoGen;
        public float OgDamage => damage;
        public float OgCooldown => cooldown;
        public float TotalDamage; 
        public float TotalCooldown;

        public FireDoT firDot;

        private Player player;

        private Timer ammoGenTimer; 
        
        public float ammoPerSecond = 0f;

        private void Awake()
        {
            OgAmmoGen = generatePerHit;
            cooldownTimer = new Timer(cooldown, true);
            currentAmmo = maxAmmo;
            
            TotalDamage = damage; 
            TotalCooldown = cooldown;

            player = GetComponent<Player>();
            
            ammoGenTimer = new Timer(1f, false);
        }

        private void OnEnable()
        {
            cooldownTimer.Timeout += OnCooldown;
            ammoGenTimer.Timeout += OnAmmoGenTimer;
        }

        private void OnDisable()
        {
            cooldownTimer.Timeout -= OnCooldown;
            ammoGenTimer.Timeout -= OnAmmoGenTimer;
        }

        private void OnCooldown()
        {
            isReady = true;
        }

        private void Update()
        {
            cooldownTimer.Tick(Time.deltaTime);
            ammoGenTimer.Tick(Time.deltaTime);
        }

        private void Start()
        {
            ammoGenTimer.Start();
        }

        public void Shoot(Vector2 dir)
        {
            if (!isReady) return;
            if (currentAmmo <= 0) return;
            
            var inst = Instantiate(projPrefab, transform.position, Quaternion.identity);
            inst.Init(dir, speed, TotalDamage, knockbackForce);
            inst.RotateTo(dir, 0);
            inst.AddDoTEffect(firDot);

            if (ExplosiveDamage > 0)
            {
                inst.AddExplosive(explosvieAttackPrefab, ExplosiveDamage, ExplosiveRadiusScale);
            }
            
            cooldownTimer.UpdateWaitTime(TotalCooldown);
            cooldownTimer.Start();
            isReady = false;

            currentAmmo -= consumePerShot;
            
            player.PlayShotSound();
            
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
        
        public void GenerateAmmo(float value)
        {
            if (currentAmmo == maxAmmo) return;
            
            generationProgress += value;
            
            if (generationProgress >= 1f)
            {
                var amount = (int)generationProgress;
                currentAmmo += amount;
                generationProgress -= amount;
                
                currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
                
                OnAmmoChanged?.Invoke(currentAmmo);
            }
        }

        public void OnAmmoGenTimer()
        {
            if (ammoPerSecond == 0) return;
            GenerateAmmo(ammoPerSecond);
        }
        
    }
}