using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Enemies._3_TankEnemy;
using UnityEngine;

public class GolemAnimatorEvents : MonoBehaviour
{
    private TankEnemy parent;

    private void Awake()
    {
        parent = GetComponentInParent<TankEnemy>();
    }

    public void Attack()
    {
        parent.ExecuteAttack();
    }

    public void EndAttack()
    {
        parent.AttackFinished();
    }
}
