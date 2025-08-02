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

    private void Start()
    {
        relicEffect = GetComponent<RelicEffect>();
        desc = GetComponentInChildren<RelicDescription>();
        cost = Random.Range(minCost, maxCost);

        if (relicEffect is RangedRelicEffect)
        {
            desc.OnEnter += UpdateIgniteRelicDesc;
        }

        costText.text = cost.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Inventory inventory = other.GetComponent<Inventory>();
        var player = other.GetComponent<Player>();

        if (inventory.coins < cost) return; 
        
        
        inventory.coins -= cost;
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        desc.gameObject.SetActive(false);
        
        
        if (relicEffect)
            relicEffect.Apply(player);
    }

    private void UpdateIgniteRelicDesc()
    {
        if (GameManager.instance.Player.rangedModifiers.fireDot.Damage != 0)
        {
            desc.description = "Ignite deals More Damage";
        }
    }
}