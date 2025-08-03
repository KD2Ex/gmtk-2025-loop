using Entities.OrbitDrone;

namespace Entities.RelicEffects
{
    public class OrbitRelicEffect : RelicEffect
    {
        public OrbitModifierType modifierType;
        public float value;
        
        public override void Apply(Player player)
        {
            player.orbit.AddModifier(modifierType, value);
        }
    }
}