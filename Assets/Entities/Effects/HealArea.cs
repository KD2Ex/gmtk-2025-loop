using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealArea : MonoBehaviour
{
    [SerializeField] public float amount = 50f;
    
    private int healsCount = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (healsCount <= 0) return;
        
        var player = other.GetComponent<Player>();
        player.Health.Add(amount);
        healsCount--;
    }

    public void Refill()
    {
        healsCount = 1;
    }
}
