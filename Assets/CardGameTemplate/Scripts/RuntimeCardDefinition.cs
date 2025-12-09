using System;
using UnityEngine;

namespace CardGameTemplate
{
    public class RuntimeCardDefinition
    {
        private Guid _runtimeCardGuid;
        [SerializeField] private string _cardName;
        [SerializeField] private string _description;
        [SerializeField] private ICardBehaviour[] _effects;
        

        public Guid RuntimeCardGuid => _runtimeCardGuid;
        public string CardName => _cardName;
        public string Description => _description;
        public ICardBehaviour[] Effects => _effects;


        public RuntimeCardDefinition(string cardName, string description, ICardBehaviour[] cardEffects)
        {
            _runtimeCardGuid = Guid.NewGuid();

            _cardName = cardName ?? throw new ArgumentNullException(nameof(cardName));
            _description = description ?? throw new ArgumentNullException(nameof(description));
            _effects = cardEffects ?? throw new ArgumentNullException(nameof(cardEffects));

            CardGameTemplate.Debug.Log(Debug.Category.Data, $"{GetType()} created with Guid: {_runtimeCardGuid}");
        }
    }
}
