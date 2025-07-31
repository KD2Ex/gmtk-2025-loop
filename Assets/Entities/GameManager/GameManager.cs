using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Enemies;
using Entities.Enemies._1_TouchEnemy;
using Entities.Enemies._2_RangedEnemy;
using Entities.Enemies._3_TankEnemy;
using Entities.Enemies._4_DashEnemy;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyType
{
    Touch = 0,
    Ranged,
    Tank,
    Dash,
    Explosive
}

[System.Serializable]
public class EnemyTypePrefab
{
    public EnemyType type;
    public Enemy prefab;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private List<EnemySpawner> spawners;
    // [SerializeField] private TouchEnemy touchEnemy;
    // [SerializeField] private RangedEnemy rangedEnemy;
    // [SerializeField] private TankEnemy tankEnemy;
    // [SerializeField] private DashEnemy dashEnemy;
    // [SerializeField] private ExplosiveEnemy explosiveEnemy;

    [SerializeField] private List<EnemyTypePrefab> enemyPrefabs;

    private Dictionary<EnemyType, Enemy> enemies;
    
    private List<RangeInt> touchEnemyAmountRanges = new ()
    {
        new RangeInt(1, 1), 
        new RangeInt(1, 2), 
        new RangeInt(2, 3), 
    };
    private List<RangeInt> rangedEnemyAmountRanges = new()
    {
        new RangeInt(1, 1),
        new RangeInt(1, 1),
        new RangeInt(1, 1),
    };
    private List<RangeInt> tankEnemyAmountRanges = new()
    {
        new RangeInt(0, 0),
        new RangeInt(1, 1),
        new RangeInt(1, 1),
    };
    private List<RangeInt> dashEnemyAmountRanges = new()
    {
        new RangeInt(0, 0),
        new RangeInt(0, 0),
        new RangeInt(1, 1),
    };
    private List<RangeInt> explosiveEnemyAmountRanges = new()
    {
        new RangeInt(0, 0),
        new RangeInt(0, 0),
        new RangeInt(1, 1),
    };

    private List<List<RangeInt>> enemyRanges = new();
    
    private List<int> rangedEnemyAmounts = new() { 1, 1, 2, 2, 2, 3, 3, 3, 4, 5, 5, 5, 5, 5, 6 };

    private List<int> tankEnemyAmounts = new() { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 4, 5, 7, 8, 10 };
    private List<int> dashEnemyAmounts = new() { 0, 0, 0, 1, 1, 2, 2, 2, 3, 3, 3, 4, 5, 5, 5 };
    private List<int> suicideEnemyAmounts = new() { 0, 0, 0, 0, 1, 1, 1, 2, 3, 3, 3, 4, 5, 5, 5 };

    private List<RangeInt> totalSpawnAmounts = new()
    {
        new RangeInt(2, 2),
    };

    public int DifficultyLevel;
      
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        
        Destroy(gameObject);
        
        
        // enemies.Add(EnemyType.Touch, touchEnemy);
        // enemies.Add(EnemyType.Ranged, rangedEnemy);
        // enemies.Add(EnemyType.Tank, tankEnemy);
        // enemies.Add(EnemyType.Dash, dashEnemy);
        // enemies.Add(EnemyType.Explosive, explosiveEnemy);
    }
    

    public void InitLevel()
    {
        foreach (var spawner in spawners)
        {
            var typesAmount = 2;
            if (DifficultyLevel >= 3)
            {
                typesAmount = 3;
            }

            var indexes = new List<int>() { 0, 1, 2, 3, 4 };

            for (int i = 0; i < typesAmount; i++)
            {
                var index = Random.Range(0, indexes.Count);
                var rangeIndex = indexes[index];

                var range = enemyRanges[rangeIndex];
                if (range[DifficultyLevel].end == 0) continue;
                //InitEnemy(spawner, range, enemies[(EnemyType)rangeIndex]);
                InitEnemy(spawner, range, enemyPrefabs[rangeIndex].prefab);
                
                indexes.RemoveAt(index);
            }
            
            // InitEnemy(spawner, touchEnemyAmountRanges, touchEnemy);
            // InitEnemy(spawner, rangedEnemyAmountRanges, rangedEnemy);
            // InitEnemy(spawner, tankEnemyAmountRanges, tankEnemy);
            // InitEnemy(spawner, dashEnemyAmountRanges, dashEnemy);
            // InitEnemy(spawner, explosiveEnemyAmountRanges, explosiveEnemy);
        }
    }

    private void InitEnemy(EnemySpawner spawner, List<RangeInt> ranges, Enemy enemy)
    {
        var range = ranges[DifficultyLevel];
        var amount = Random.Range(range.start, range.end); 
                
        spawner.Spawn(enemy, amount);
    }


    // Start is called before the first frame update
    void Start()
    {
        
        enemyRanges.Add(touchEnemyAmountRanges);
        enemyRanges.Add(rangedEnemyAmountRanges);
        enemyRanges.Add(tankEnemyAmountRanges);
        enemyRanges.Add(dashEnemyAmountRanges);
        enemyRanges.Add(explosiveEnemyAmountRanges);
        
        InitLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     
}
