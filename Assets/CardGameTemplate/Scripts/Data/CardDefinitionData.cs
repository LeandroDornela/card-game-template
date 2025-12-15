using UnityEngine;

namespace CardGameTemplate
{
    [System.Serializable]
    public struct CardDefinitionData
    {
        [SerializeField] private string _cardName;
        [SerializeField] private string _description;
        [SerializeField] private CardBehaviourData[] _effects;
        

        public string CardName => _cardName;
        public string Description => _description;
        public CardBehaviourData[] Effects => _effects;

        public CardDefinitionData(string cardName, string description, CardBehaviourData[] effects)
        {
            _cardName = cardName;
            _description = description;
            _effects = effects;
        }
    }
}
