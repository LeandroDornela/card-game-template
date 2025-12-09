using UnityEngine;

namespace CardGameTemplate
{
    [CreateAssetMenu(fileName = "SOCardDefinition", menuName = "Scriptable Objects/SOCardDefinition")]
    public class SOCardDefinition : ScriptableObject
    {
        [SerializeField] private CardDefinition _cardDefinition;

        public CardDefinition CardDefinition => _cardDefinition;
    }
}
