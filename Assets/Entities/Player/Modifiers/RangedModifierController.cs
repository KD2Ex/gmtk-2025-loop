using System;
using UnityEngine;

public enum RangedModifierType
{
    Damage,
    Cooldown
}

namespace Entities.Modifiers
{
    public class RangedModifierController : ModifiersController
    {

        public float baseDamage;
        public float cooldown;
        
        public void AddModifier(RangedModifierType type, Modifier modifier)
        {
            switch (type)
            {
                case RangedModifierType.Damage:
                    baseDamage += modifier.value;
                    break;
                case RangedModifierType.Cooldown:
                    cooldown += modifier.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            player.UpdateRangedStats();
        }

        public float GetTotalValue(RangedModifierType type, float baseValue, float increased = 0f)
        {
            var totalBase = 0f;
            var result = 0f;
            
            switch (type)
            {
                case RangedModifierType.Damage:
                    totalBase = baseValue + baseDamage;
                    result = totalBase + totalBase * (increased * 0.01f);
                    return result;
                case RangedModifierType.Cooldown:
                    totalBase = baseValue;
                    result = totalBase - totalBase * (cooldown * 0.01f);
                    return result;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}