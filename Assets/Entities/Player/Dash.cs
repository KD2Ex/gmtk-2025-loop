using System;
using System.Collections;
using UnityEngine;

namespace Entities
{
    public class Dash : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float speed;
        [SerializeField] private float time;

        private bool isDashing = false;
        private float elapsed = 0f;
        public bool IsDashing => isDashing;
        public float TimeRemain => elapsed / time;

        public Action Finished;

        public void Execute(Vector2 dir)
        {
            isDashing = true;
            StopAllCoroutines();
            StartCoroutine(Dashing(dir));
        }

        private IEnumerator Dashing(Vector2 dir)
        {
            elapsed = 0f;

            while (elapsed < time)
            {
                var curveValue = curve.Evaluate(elapsed / time);
                var vel = dir * (speed * curveValue);
                rb.velocity = vel;
                print(rb.velocity.magnitude);
                yield return Time.deltaTime;
                elapsed += Time.deltaTime;
            }

            isDashing = false;
            Finished?.Invoke();
        }
        
    }
}