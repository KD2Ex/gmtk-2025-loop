using System;
using UnityEngine;

namespace Health
{
    public class HealthComponent : MonoBehaviour
    {
        public float Value;
        public float MaxValue;

        public bool isDead;

        public Action<float, float> OnValueChanged;
        public Action OnDeath;

        public void Remove(float value)
        {
            Value -= value;
            
            OnValueChanged?.Invoke(Value, MaxValue);

            if (Value <= 0)
            {
                isDead = true;
                OnDeath?.Invoke();
            }
        }

        public void Add(float value)
        {
            Value += value;
            
            Value = Mathf.Clamp(Value, 0f, MaxValue);
            
            OnValueChanged?.Invoke(Value, MaxValue);
        }
    }
}