using System;
using System.Collections;
using System.Collections.Generic;
using Health;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HPText : MonoBehaviour
{
    [SerializeField] private HealthComponent health;
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        UpdateHP(health.Value, health.MaxValue);
    }

    private void OnEnable()
    {
        health.OnValueChanged += UpdateHP;
    }

    private void OnDisable()
    {
        health.OnValueChanged -= UpdateHP;
    }

    private void UpdateHP(float value, float maxValue)
    {
        text.text = $"{(int)value} / {(int)maxValue}";
    }
}
