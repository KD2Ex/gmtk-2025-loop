using System;
using TMPro;
using UnityEngine;

public class Well : MonoBehaviour
{
    [SerializeField] private float healAmount;
    [SerializeField] private int cost;
    [SerializeField] private TMP_Text textCost;
    [Multiline] [SerializeField] private string warningDesc;
    [Multiline] [SerializeField] private string healedDesc;
    [Multiline] [SerializeField] private string fullHPDesc;

    [SerializeField] private RelicDescription relicDescription;

    public float currentCost;
    public int MaxCost = 500;

    private void Start()
    {
        UpdateCost();
    }

    private void OnEnable()
    {
        relicDescription.OnEnter += OnDescEnter;
        GameManager.instance.OnHubEnter += UpdateCost;
    }

    private void OnDisable()
    {
        relicDescription.OnEnter -= OnDescEnter;
        GameManager.instance.OnHubEnter -= UpdateCost;
    }

    private void UpdateCost()
    {
        currentCost = cost * GameManager.instance.GetEnemyScale();
        currentCost = Mathf.Clamp(currentCost, 0, MaxCost);
        
        textCost.text = ((int) currentCost).ToString();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();

        if (Mathf.Approximately(player.Health.Value, player.Health.MaxValue))
        {
            return;
        }
        
        var inv = player.GetComponent<Inventory>();

        if (inv.coins - cost >= 0)
        {
            player.Health.Add(player.Health.MaxValue / 2);
            inv.coins -= (int)currentCost;
            relicDescription.description = healedDesc;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (Mathf.Approximately(player.Health.Value, player.Health.MaxValue))
        {
            relicDescription.description = fullHPDesc;
            return;
        }
        
        relicDescription.description = warningDesc;
    }

    private void OnDescEnter(Player player)
    {

        if (Mathf.Approximately(player.Health.Value, player.Health.MaxValue))
        {
            relicDescription.description = fullHPDesc;
        }
        else
        {
            
            relicDescription.description = warningDesc;
        }
    }

}
