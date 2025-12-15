using UnityEngine;

namespace CardGameTemplate
{
    [CreateAssetMenu(fileName = "NewCardDefinition", menuName = "Card Game Template/Card Definition")]
    public class SOCardDefinition : ScriptableObject
    {
        [SerializeField] private CardDefinitionData _cardDefinition;

        public CardDefinitionData CardDefinition => _cardDefinition;
    }
}
