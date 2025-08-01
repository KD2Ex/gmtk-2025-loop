using System;
using Entities.Modifiers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities.RelicEffects
{
    public class MeleeRelicEffect : RelicEffect
    {
        [FormerlySerializedAs("modifierType")] [SerializeField] private MeleeModifierType meleeModifierType;
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
            player.meleeModifiers.AddModifier(meleeModifierType, modifier);
        }
    }
}