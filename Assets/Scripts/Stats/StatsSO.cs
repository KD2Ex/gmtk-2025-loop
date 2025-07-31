using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsSO")]

public class StatsSO : ScriptableObject
{
    public float health;
    public float damage;
    public float moveSpeed;
    public float attackDelay;

    [ContextMenu("Reset Stats")]
    public void ResetStats()
    {
        health = 0;
        damage = 0;
        moveSpeed = 0;
        attackDelay = 0;
    }
}
