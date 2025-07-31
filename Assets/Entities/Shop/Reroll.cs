using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Reroll : MonoBehaviour
{
    public int cost;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private int deltaCost;

    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject relicPrefab;

    private void Start()
    {
        costText.text = cost.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Inventory inventory = other.GetComponent<Inventory>();
        
        
        if (inventory.coins < cost) return; 
        inventory.coins -= cost;
    }
}
