using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private CircleCollider2D spawnArea;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int amount = 3;
    [SerializeField] private bool spawnOnStart = true;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!spawnOnStart) return;
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Spawn()
    {
        for (int i = 0; i < amount; i++)
        {
            var point = Random.insideUnitCircle * spawnArea.radius;
            var pos = transform.position + (Vector3)point;
            TryToInst(pos);
        }
        
    }

    private void TryToInst(Vector3 position)
    {
        var occupied = true;
        for (int i = 0; i < 10; i++)
        {
            var coll = Physics2D.OverlapCircle(position, 2f, LayerMask.GetMask("Enemy"));
            if (coll) continue;
            
            occupied = false;
            break;
        }

        if (occupied) return;
        var inst = Instantiate(enemyPrefab,  position, Quaternion.identity);
        
    }
}

