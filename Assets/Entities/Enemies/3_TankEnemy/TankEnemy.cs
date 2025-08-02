using System;
using System.Collections;
using Attacks;
using Damage;
using Health;
using Sensors;
using UnityEngine;

namespace Entities.Enemies._3_TankEnemy
{
    public class TankEnemy : Enemy, IDamageable
    {
        [SerializeField] private EnemySensor chaseSensor;
        [SerializeField] private EnemySensor attackSensor;
        [SerializeField] private Transform attackPivot;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float attackDelayTime = .4f;
        [SerializeField] private float attackRestoreTime = .4f;
        [SerializeField] private float attackCooldownTime = 1f;

        private Player player;

        private bool playerInAttackRange = false;
        private bool blockMovement = false;
        private bool attackCharging = false;


        private bool isAttacking;

        private Vector2 lastVelocity;

        protected override void Awake()
        {
            base.Awake();
            

            attack.damage = damage;
            attack.knockbackForce = knockbackForce;
        }

        private void Update()
        {
            // attackDelay.Tick(Time.deltaTime);
            // attackRestore.Tick(Time.deltaTime);

            sprite.flipX = lastVelocity.x > 0;
        }

        private void FixedUpdate()
        {
            if (health.isDead) return;
            if (!player) return;
            
            //print(isAttacking + " " + playerInAttackRange);
            if (isAttacking || playerInAttackRange)
            {
                rb.velocity = Vector2.zero;
                return;
            }
            
            Chase();

            if (rb.velocity != Vector2.zero)
            {
                lastVelocity = rb.velocity;
            }
        }

        private void OnEnable()
        {
            chaseSensor.OnEnter += OnChaseSensorEnter;
            attackSensor.OnEnter += OnAttackSensorEnter;
            attackSensor.OnLeave += OnAttackSensorLeave;

        }

        private void OnDisable()
        {
            chaseSensor.OnEnter -= OnChaseSensorEnter;
            attackSensor.OnEnter -= OnAttackSensorEnter;
            attackSensor.OnLeave -= OnAttackSensorLeave;
        }

        private void OnChaseSensorEnter(Player player)
        {
            this.player = player;
        }

        private void OnAttackSensorEnter(Player player)
        {
            //print("Attack sensor entered");
            if (isAttacking) return;
            playerInAttackRange = true;
            
            // attackDelay.Start(); // start attack animation
            // attackCharging = true;
            //var dir = (player.transform.position - transform.position).normalized;
            //attack.RotateToTarget(dir, attackPivot);

            Attack();
            rb.velocity = Vector2.zero;
        }
        
        private void OnAttackSensorLeave(Player player)
        {
            playerInAttackRange = false;

        }

        private void Attack()
        {
            animator.Play("GolemAttack", 0, 0f);
            isAttacking = true;
        }

        public void ExecuteAttack()
        {
            attack.Execute(damage);
        }

        public void AttackFinished()
        {
            StartCoroutine(Wait(.5f));
        }

        private IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            isAttacking = false;

            if (playerInAttackRange)
            {
                Attack();
            }
        } 
        
        private void Chase()
        {
            var dir = (player.transform.position - transform.position).normalized;
            rb.velocity = dir * moveSpeed;
            
            animator.Play("GolemWalk");
        }

        public void TakeDamage(DamageMessage message)
        {
            if (health.isDead) return;
            health.Remove(message.damage);
            ShowDamageText(Mathf.RoundToInt(message.damage));
            StartCoroutine(Flash());
            
            // if (health.isDead)
            // {
            //     Die();
            //     return;
            // }
            
            //print(health.Value);
            
            //StopAllCoroutines();
        }


        protected override void Die()
        {
            chaseSensor.OnEnter -= OnChaseSensorEnter;
            attackSensor.OnEnter -= OnAttackSensorEnter;
            attackSensor.OnLeave -= OnAttackSensorLeave;
            StopAllCoroutines();
            
            sprite.material.SetFloat("_Amount", 0);
            
            isAttacking = false;
            
            rb.excludeLayers = LayerMask.GetMask("Player", "Ignore Raycast", "Enemy", "Default");
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            sprite.sortingOrder = -1;
            
            animator.Play("GolemDeath", 0, 0f);
        }
    }
}