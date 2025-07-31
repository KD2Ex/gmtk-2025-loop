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
    
    [SerializeField] private List<Enemy> prefabs = new ();
    [SerializeField] private List<int> amounts = new ();
    
    [SerializeField] private bool spawnOnStart = true;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!spawnOnStart) return;
        SpawnAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(Enemy pref, int amount)
    {
        for (int j = 0; j < amount; j++)
        {
            var point = Random.insideUnitCircle * spawnArea.radius;
            var pos = transform.position + (Vector3)point;
            TryToInst(pos, pref);
        }
    }

    private void SpawnAll()
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            for (int j = 0; j < amounts[i]; j++)
            {
                var point = Random.insideUnitCircle * spawnArea.radius;
                var pos = transform.position + (Vector3)point;
                TryToInst(pos, prefabs[i]);
            }
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < amount; i++)
        {
            var point = Random.insideUnitCircle * spawnArea.radius;
            var pos = transform.position + (Vector3)point;
            TryToInst(pos, enemyPrefab);
        }
        
    }

    private void TryToInst(Vector3 position, Enemy prefab)
    {
        var occupied = true;
        for (int i = 0; i < 50; i++)
        {
            var coll = Physics2D.OverlapCircle(position, 1f, LayerMask.GetMask("Enemy"));
            if (coll) continue;
            
            occupied = false;
            break;
        }

        if (occupied) return;
        var inst = Instantiate(prefab,  position, Quaternion.identity);
        
    }
}