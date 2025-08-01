using Entities.Modifiers;
using UnityEngine;

namespace Entities.RelicEffects
{
    public class PlayerRelicEffect : RelicEffect
    {
        
        public PlayerModifierType modifierType;
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
            player.playerModifiers.AddModifier(modifierType, modifier);
        }
    }
}