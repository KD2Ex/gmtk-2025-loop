using System;
using System.Collections;
using System.Collections.Generic;
using Entities.RelicEffects;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Relic : MonoBehaviour
{
    public int cost;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private int minCost;
    [SerializeField] private int maxCost;
    
    private RelicEffect relicEffect;
    private RelicDescription desc;

    private string ogDesc;

    private void Start()
    {
        relicEffect = GetComponent<RelicEffect>();
        desc = GetComponentInChildren<RelicDescription>();
        
        
        
        cost = Random.Range(minCost, maxCost);

        if (GameManager.instance.DifficultyLevel > 1)
        {
            cost += (int)((float)cost * GameManager.instance.DifficultyLevel * 0.1f);
        }

        if (relicEffect is RangedRelicEffect)
        {
            desc.OnEnter += UpdateIgniteRelicDesc;
        }

        if (relicEffect is OrbitRelicEffect)
        {
            desc.OnEnter += UpdateOrbitRelicDesc;
            ogDesc = desc.description;
        }

        costText.text = cost.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Inventory inventory = other.GetComponent<Inventory>();
        var player = other.GetComponent<Player>();

        if (inventory.coins < cost) return; 
        
        
        inventory.coins -= cost;
        
        //gameObject.GetComponent<Collider2D>().enabled = false;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        Destroy(gameObject);
        
        desc.gameObject.SetActive(false);
        
        
        if (relicEffect)
            relicEffect.Apply(player);
    }

    private void UpdateIgniteRelicDesc(Player _)
    {
        if (GameManager.instance.Player.rangedModifiers.fireDot.Damage != 0)
        {
            desc.description = "Ignite deals More Damage";
        }
    }

    private void UpdateOrbitRelicDesc(Player player)
    {
        if (!player.orbit.IsRunning)
        {
            desc.description = "Orbit flame appears in Loop\n" + ogDesc;
        }
    }
}