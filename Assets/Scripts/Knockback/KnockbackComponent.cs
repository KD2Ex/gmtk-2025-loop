using System;
using UnityEngine;

namespace Knockback
{
    public class KnockbackComponent : MonoBehaviour
    {
        public Action<Vector2, float> velocitySetter;

        private float time = .3f;
        private float elapsed = 0f;

        private Vector2 dir;
        private float force;
        private float currentForce;

        private bool exec = false;

        public bool IsRunning => exec;
        
        public void Execute(Vector2 dir, float force, float time = 0.3f)
        {
            //print("Knockback exec");
            exec = true;
            this.dir = dir;
            this.force = force;
            this.time = time;
            currentForce = force;
        }

        private void FixedUpdate()
        {
            if (!exec) return;

            elapsed += Time.deltaTime;
            
            velocitySetter?.Invoke(dir, currentForce);
            
            currentForce = force * (time - elapsed) / time;

            if (elapsed >= time)
            {
                exec = false;
                elapsed = 0.0f;
                currentForce = 0f;
            }
        }
    }
}