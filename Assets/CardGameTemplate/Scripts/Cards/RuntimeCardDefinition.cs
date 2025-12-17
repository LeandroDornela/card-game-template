using System;
using UnityEngine;

namespace CardGameTemplate
{
    public class RuntimeCardDefinition : IBehaviourTarget
    {
        private Guid _runtimeCardGuid;
        [SerializeField] private string _cardName;
        [SerializeField] private string _description;
        [SerializeField] private CardBehaviour[] _behaviours;
        

        public Guid RuntimeCardGuid => _runtimeCardGuid;
        public string CardName => _cardName;
        public string Description => _description;
        public CardBehaviour[] Behaviours => _behaviours;


        public RuntimeCardDefinition(string cardName, string description, CardBehaviour[] cardEffects)
        {
            _runtimeCardGuid = Guid.NewGuid();

            _cardName = cardName ?? throw new ArgumentNullException(nameof(cardName));
            _description = description ?? throw new ArgumentNullException(nameof(description));
            _behaviours = cardEffects ?? throw new ArgumentNullException(nameof(cardEffects));

            foreach(var behaviour in _behaviours)
            {
                behaviour.RuntimeCardGuid = _runtimeCardGuid;
            }

            Debug.Log(Debug.Category.Data, $"{GetType()} created with Guid: {_runtimeCardGuid}");
        }
    }
}
