
using System;

public enum DashModifierType
{
    Speed,
    Cooldown,
    FlameDash,
    Vulnerability
}

namespace Entities.Modifiers
{
    
    public class DashModifiersController : ModifiersController
    {
        public float dashSpeed;
        public float cooldown;

        public void AddModifier(DashModifierType type, Modifier modifier)
        {
            
            switch (type)
            {
                case DashModifierType.Speed:
                    dashSpeed += modifier.value;
                    break;
                case DashModifierType.Cooldown:
                    cooldown += modifier.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            player.UpdateDashStats();
        }
        
        public float GetTotalValue(DashModifierType type, float baseValue, float increased = 0f)
        {
            var totalBase = 0f;
            var result = 0f;
            
            switch (type)
            {
                case DashModifierType.Speed:
                    totalBase = baseValue + dashSpeed;
                    result = totalBase + totalBase * (increased * 0.01f);
                    return result;
                case DashModifierType.Cooldown:
                    totalBase = baseValue;
                    result = totalBase - totalBase * (cooldown * 0.01f);
                    return result;
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}