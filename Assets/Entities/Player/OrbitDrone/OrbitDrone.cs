using System;
using Attacks;
using Damage;
using UnityEngine;

namespace Entities.OrbitDrone
{
    public class OrbitDrone: MonoBehaviour
    {
        public float rotationSpeed = 5f;
        public Attack attack;
        private void FixedUpdate()
        {
            
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        }

        private void OnEnable()
        {
            attack.OnHit += OnHit;
        }

        private void OnDisable()
        {
            attack.OnHit -= OnHit;
        }

        private void OnHit(Collider2D other)
        {
            print($"Orbit dealt {attack.damage} to {other.name}");
        }
    }
}