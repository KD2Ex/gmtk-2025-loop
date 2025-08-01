using Entities.Modifiers;
using UnityEngine;

namespace Entities.RelicEffects
{
    public class RangedRelicEffect : RelicEffect
    {
        public RangedModifierType type;
        [SerializeField] private float modifierValue;

        private Modifier modifier;

        private void Awake()
        {
            modifier = new Modifier
            {
                value = modifierValue
            };
        }

        public override void Apply(Player player)
        {
            player.rangedModifiers.AddModifier(type, modifier);
        }
    }
}