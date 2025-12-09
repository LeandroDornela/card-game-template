using System;
using UnityEngine;

namespace CardGameTemplate
{
    [System.Serializable]
    public struct CardDefinition
    {
        [SerializeField] private string _cardName;
        [SerializeField] private string _description;
        [SerializeField] private BehaviourDefinition[] _effects;
        

        public string CardName => _cardName;
        public string Description => _description;
        public BehaviourDefinition[] Effects => _effects;

        public CardDefinition(string cardName, string description, BehaviourDefinition[] effects)
        {
            _cardName = cardName;
            _description = description;
            _effects = effects;
        }
    }
}
