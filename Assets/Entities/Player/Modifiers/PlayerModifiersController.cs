using System;
using UnityEngine;

public enum PlayerModifierType
{
    HP,
    MoveSpeed
}

namespace Entities.Modifiers
{
    public class PlayerModifiersController : ModifiersController
    {
        public float maxHP;
        public float baseMoveSpeed;
        
        public void AddModifier(PlayerModifierType type, Modifier modifier)
        {

            switch (type)
            {
                case PlayerModifierType.HP:
                    maxHP += modifier.value;
                    break;
                case PlayerModifierType.MoveSpeed:
                    baseMoveSpeed += modifier.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
           
            // player.UpdatePlayerStats();
        }
        
        public float GetTotalValue(PlayerModifierType type, float baseValue, float increased = 0f)
        {
            var totalBase = 0f;
            var result = 0f;
            
            switch (type)
            {
                case PlayerModifierType.HP:
                    totalBase = baseValue + maxHP;
                    result = totalBase + totalBase * (increased * 0.01f);
                    return result;
                case PlayerModifierType.MoveSpeed:
                    totalBase = baseValue + baseMoveSpeed;
                    result = totalBase + totalBase * (increased * 0.01f);
                    return result;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}