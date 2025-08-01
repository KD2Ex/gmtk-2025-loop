using Entities.Modifiers;
using UnityEngine;

namespace Entities.RelicEffects
{
    public class DashRelicEffect : RelicEffect
    {
        public DashModifierType type;
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
            player.dashModifiers.AddModifier(type, modifier);
        }
    }
}