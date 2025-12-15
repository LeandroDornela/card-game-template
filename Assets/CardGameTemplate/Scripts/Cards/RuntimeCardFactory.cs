using System.ComponentModel;

namespace CardGameTemplate
{
    public class RuntimeCardFactory
    {
        public static bool TryCreateRuntimeCardDefinition(CardDefinitionData cardDefinition, out RuntimeCardDefinition runtimeCardDefinition)
        {
            CardBehaviour[] cardEffects;

            if(!TryCreateRuntimeEffects(cardDefinition.Effects, out cardEffects))
            {
                runtimeCardDefinition = default;
                return false;
            }

            runtimeCardDefinition = new RuntimeCardDefinition(cardDefinition.CardName, cardDefinition.Description, cardEffects);

            return true;
        }
        
        public static bool TryCreateRuntimeEffects(CardBehaviourData[] effectDefinitions, out CardBehaviour[] cardEffects)
        {
            // Check input effects array.
            if(effectDefinitions == null || effectDefinitions.Length == 0)
            {
                Debug.LogError(Debug.Category.Data, $"{nameof(effectDefinitions)} has no elements.");
                cardEffects = null;
                return false;
            }
            
            // Fill effects array with new ICardEffects created from Card Definitions.
            cardEffects = new CardBehaviour[effectDefinitions.Length];
            for(int i = 0; i < cardEffects.Length; i++)
            {
                if(!TryCreateRuntimeEffect(effectDefinitions[i], out cardEffects[i]))
                {
                    // If fails to create one effect, returns as fail.
                    cardEffects = null;
                    return false;
                }
            }
            return true;
        }
        
        // Instantiate a ICardEffect, e.g. "EffectDamageHealth:ICardEffect", based on the effect tye defined at the card definition struct.
        public static bool TryCreateRuntimeEffect(CardBehaviourData effectDefinition, out CardBehaviour cardEffect)
        {
            switch(effectDefinition.BehaviourType)
            {
                case BehaviourType.DamageHealth:
                    cardEffect = new BehaviourDamageHealth(effectDefinition.BehaviourType.ToString(), effectDefinition.MainValue, effectDefinition.TargetType);
                    return true;
                /*
                case EffectType.RecoverHealth:
                    cardEffect = null;
                    throw new NotImplementedException();
                case EffectType.Combat:
                    cardEffect = null;
                    throw new NotImplementedException();
                */
                default:
                    cardEffect = null;
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}
