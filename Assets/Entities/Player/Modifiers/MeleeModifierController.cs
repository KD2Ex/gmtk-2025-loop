using System;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeModifierType
{
    Damage,
    Radius,
    Knockback,
    AmmoGeneration,
    AddSlash
}

namespace Entities.Modifiers
{
    public class MeleeModifierController : ModifiersController
    {
        // private List<Modifier> damageModifiers = new();
        // private List<Modifier> radiusModifiers = new();
        // private List<Modifier> knockbackModifiers = new();
        // private List<Modifier> ammoGenerationModifiers = new();

        public float baseDamage;
        public float radius;
        public float knockback;
        public float ammoGeneration;

        public float TotalDamage;

        public void AddModifier(MeleeModifierType type, Modifier modifier)
        {
            switch (type)
            {
                case MeleeModifierType.Damage:
                    baseDamage += modifier.value;
                    break;
                case MeleeModifierType.Radius:
                    radius += modifier.value;
                    break;
                case MeleeModifierType.Knockback:
                    knockback += modifier.value;
                    break;
                case MeleeModifierType.AmmoGeneration:
                    ammoGeneration += modifier.value;
                    break;
                case MeleeModifierType.AddSlash:
                    player.AddSlash();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            player.UpdateAttackStats();
        }

        public float GetTotalValue(MeleeModifierType type, float baseValue, float increased = 0f)
        {
            var totalBase = 0f;
            var result = 0f;
            
            switch (type)
            {
                case MeleeModifierType.Damage:
                    totalBase = baseValue + baseDamage;
                    result = totalBase + totalBase * (increased * 0.01f);
            
                    return result;
                case MeleeModifierType.Radius:
                    totalBase = 1;
                    result = totalBase + totalBase * (radius * 0.01f);
                    
                    return result;
                case MeleeModifierType.Knockback:
                    totalBase = baseValue;
                    result = totalBase + totalBase * (knockback * 0.01f);
                    
                    return result;
                case MeleeModifierType.AmmoGeneration:
                    totalBase = baseValue + ammoGeneration;
                    result = totalBase;
                    
                    return result;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
    }
}