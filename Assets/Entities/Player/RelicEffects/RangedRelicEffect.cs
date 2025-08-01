using Entities.DoTEffects;
using Entities.Modifiers;
using UnityEngine;

namespace Entities.RelicEffects
{
    public class RangedRelicEffect : RelicEffect
    {
        public RangedModifierType type;
        [SerializeField] private float modifierValue;
        [SerializeField] private FireDoT fireDot;

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
            if (type == RangedModifierType.FireDOT)
            {
                player.rangedModifiers.AddFireDoT(fireDot);
                return;
            }
            player.rangedModifiers.AddModifier(type, modifier);
        }
    }
}