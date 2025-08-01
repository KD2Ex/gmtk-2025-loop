using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Enemies;
using Entities.Enemies._1_TouchEnemy;
using Entities.Enemies._2_RangedEnemy;
using Entities.Enemies._3_TankEnemy;
using Entities.Enemies._4_DashEnemy;
using TMPro;
using Unity.Collections;
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
    public bool spawnOnStart;

    public TMP_Text difficultyText;

    [SerializeField] private List<EnemySpawner> spawners;

    [SerializeField] private List<EnemyTypePrefab> enemyPrefabs;

    private Dictionary<EnemyType, Enemy> enemies;
    
    private List<RangeInt> touchEnemyAmountRanges = new ()
    {
        new RangeInt(2, 2), 
        new RangeInt(3, 1), 
        new RangeInt(3, 2), 
        new RangeInt(4, 1), 
        new RangeInt(4, 2), 
        new RangeInt(5, 1), 
        new RangeInt(6, 1), 
        new RangeInt(6, 2), 
    };
    private List<RangeInt> rangedEnemyAmountRanges = new()
    {
        new RangeInt(2, 1),
        new RangeInt(2, 2),
        new RangeInt(2, 3),
        new RangeInt(3, 1),
        new RangeInt(3, 2),
        new RangeInt(4, 2),
        new RangeInt(4, 3),
    };
    private List<RangeInt> tankEnemyAmountRanges = new()
    {
        new RangeInt(0, 0),
        new RangeInt(1, 0),
        new RangeInt(1, 1),
        new RangeInt(1, 2),
        new RangeInt(2, 1),
        new RangeInt(2, 2),
        new RangeInt(3, 1),
        new RangeInt(3, 2),
    };
    private List<RangeInt> dashEnemyAmountRanges = new()
    {
        new RangeInt(0, 0),
        new RangeInt(0, 0),
        new RangeInt(1, 0),
        new RangeInt(1, 1),
        new RangeInt(2, 1),
        new RangeInt(2, 1),
        new RangeInt(3, 1),
        new RangeInt(3, 1),
        new RangeInt(3, 1),
    };
    private List<RangeInt> explosiveEnemyAmountRanges = new()
    {
        new RangeInt(0, 0),
        new RangeInt(0, 0),
        new RangeInt(0, 0),
        new RangeInt(1, 0),
        new RangeInt(1, 0),
        new RangeInt(1, 1),
        new RangeInt(1, 2),
        new RangeInt(1, 2),
        new RangeInt(1, 3),
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

    private List<Enemy> spawnedEnemies = new();

    public int DifficultyLevel;
    [ReadOnly] public float Coeff;

    private int stagesCompleted = 0;
    private float stageFactor = 0;
    private int minutesPassed = 0;

    private float timeFactor = 0.0506f;
    private float playerFactor = 1f;

    private Timer difficultyTimer;
      
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            difficultyTimer = new Timer(30f,  false);
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
                if (range[DifficultyLevel].end == 0)
                {
                    i--;
                    continue;
                }
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
                
        spawner.Spawn(enemy, amount, out var spawned);
        
        spawnedEnemies.AddRange(spawned);
    }


    // Start is called before the first frame update
    void Start()
    {
       
        
        enemyRanges.Add(touchEnemyAmountRanges);
        enemyRanges.Add(rangedEnemyAmountRanges);
        enemyRanges.Add(tankEnemyAmountRanges);
        enemyRanges.Add(dashEnemyAmountRanges);
        enemyRanges.Add(explosiveEnemyAmountRanges);
        
        
        difficultyTimer.Start();
        if (spawnOnStart)
            InitLevel();
    }

    private void OnEnable()
    {
        difficultyTimer.Timeout += DifficultyTimerTimeout;
    }

    private void OnDisable()
    {
        difficultyTimer.Timeout -= DifficultyTimerTimeout;
    }

    // Update is called once per frame
    void Update()
    {
        difficultyTimer.Tick(Time.deltaTime);
    }

    private void DifficultyTimerTimeout()
    {
        minutesPassed++;
        //stagesCompleted++;
        
        CalculateDifficultyLevel();
        
    }

    public float CalculateCoeff()
    {
        CalculateStageFactor();
        var coeff = (playerFactor + timeFactor * minutesPassed) * stageFactor;

        Coeff = coeff;
        
        return coeff;
    }

    private void CalculateDifficultyLevel()
    {
        CalculateCoeff();
        DifficultyLevel = Mathf.FloorToInt(1 + (Coeff - playerFactor) / 0.25f) - 1;
        
        print("Difficulty: " + DifficultyLevel);

        if (difficultyText)
            difficultyText.text = "Difficutly: " + DifficultyLevel.ToString();
    }

    private void CalculateStageFactor()
    {
        stageFactor = Mathf.Pow(1.05f, stagesCompleted);
    }

    public void CompleteStage()
    {
        stagesCompleted++;
        CalculateDifficultyLevel();
    }

    public void EnterLoop()
    {
        InitLevel();
        print(spawnedEnemies.Count);
    }

    public void ExitLoop()
    {
        RemoveAllSpawnedEnemies();
    }

    private void RemoveAllSpawnedEnemies()
    {
        foreach (var e in spawnedEnemies)
        {
            if (e)
                Destroy(e.gameObject);
        }
        
        spawnedEnemies.Clear();
    }
}