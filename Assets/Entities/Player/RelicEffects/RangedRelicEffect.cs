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
        [SerializeField] private float explosiveDamage;
        [SerializeField] private float explosiveRadiusScale;

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

            if (type == RangedModifierType.Explosive)
            {
                player.rangedModifiers.AddExplosive(explosiveDamage, explosiveRadiusScale);
                return;
            }
            player.rangedModifiers.AddModifier(type, modifier);
        }
    }
}