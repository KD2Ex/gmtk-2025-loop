using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealArea : MonoBehaviour
{
    [SerializeField] private float amount = 50f;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        player.Health.Add(amount);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
