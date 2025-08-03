using System;
using System.Collections;
using System.Collections.Generic;
using Entities.RelicEffects;
using TMPro;
using UnityEngine;

public class RelicDescription : MonoBehaviour
{
    public string description;
    private TextMeshProUGUI textBox;

    public Action<Player> OnEnter;

    private Relic relic;

    private String ogDesc;

    private void Start()
    {
        ogDesc = description;
        textBox =  GameObject.FindWithTag("ShopTextBox").GetComponent<TextMeshProUGUI>();
        relic = GetComponentInParent<Relic>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnEnter?.Invoke(other.GetComponent<Player>());

        if (relic)
        {
            
            if (relic.relicEffect is RangedRelicEffect)
            {
                RangedRelicEffect rangedRelic = (RangedRelicEffect)relic.relicEffect;
                if (rangedRelic.type == RangedModifierType.FireDOT)
                {
                    if (GameManager.instance.Player.rangedModifiers.fireDot.Damage != 0)
                    {
                        description = "Ignite deals More Damage";
                    }
                }
            }

            if (relic.relicEffect is OrbitRelicEffect)
            {
            
                if (!GameManager.instance.Player.orbit.IsRunning)
                {
                    description = "Orbit flame appears in Loop\n" + ogDesc;
                }
            }
        }
        
        textBox.text = description;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        textBox.text = null;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        textBox.text = description;
    }
}