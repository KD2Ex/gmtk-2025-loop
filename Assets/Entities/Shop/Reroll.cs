using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Reroll : MonoBehaviour
{
    public int baseCost;
    public int cost;
    [SerializeField] private int deltaCost;
    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private List<GameObject> pedestals;

    [Header("PossibleRelics")] 
    [SerializeField] private List<GameObject> relics;

    private void OnEnable()
    {
        GameManager.instance.playerEnteredHub += ResetCost;
        GameManager.instance.playerEnteredHub += RerollRelics;
    }

    private void OnDisable()
    {
        GameManager.instance.playerEnteredHub -= ResetCost;
        GameManager.instance.playerEnteredHub -= RerollRelics;
    }

    private void Start()
    {
        cost = baseCost;
        costText.text = cost.ToString();
        RerollRelics();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Inventory inventory = other.GetComponent<Inventory>();
        if (inventory.coins < cost) return; 
        inventory.coins -= cost;
        cost *= GameManager.instance.DifficultyLevel + 2;
        costText.text = cost.ToString();
        RerollRelics();
    }

    public void ResetCost()
    {
        cost = baseCost;
        costText.text = cost.ToString();
    }
    
    private void RerollRelics()
    {
        foreach (var pedestal in pedestals)
        {
            if (pedestal.transform.childCount == 1)
                Destroy(pedestal.transform.GetChild(0).gameObject);

            var relic = GetRandomRelic();
            
            //Instantiate(relics[Random.Range(0, relics.Count)], pedestal.transform);
            Instantiate(relic, pedestal.transform);
        }
    }
    
    private List<int> randomIndexes = new();
    public GameObject GetRandomRelic()
    {
        int randIndex = 0;


        for (int i = 0; i < 10; i++)
        {
            randIndex = Random.Range(0, relics.Count);
            if (!randomIndexes.Contains(randIndex)) break;
        }
        
        print("Index of relic: " + randIndex);
        print("Name of relic: " + relics[randIndex].name);

        randomIndexes.Add(randIndex);
        
        return relics[randIndex];
    }
}
