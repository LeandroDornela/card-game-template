using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameTemplate
{
    public class RuntimePlayerProfile
    {
        private Guid _runtimePlayerGuid;
        private string _playerName;
        private PlayerType _playerType;
        private Dictionary<Guid, RuntimeCardDefinition> _deckCardDefinitions;

        public Guid RuntimePlayerGuid => _runtimePlayerGuid;
        public string PlayerName => _playerName;
        public PlayerType PlayerType => _playerType;
        public IReadOnlyDictionary<Guid, RuntimeCardDefinition> DeckCardDefinitions => _deckCardDefinitions;


        public RuntimePlayerProfile(string playerName, PlayerType playerType, RuntimeCardDefinition[] cardDefinitions)
        {
            Initialize(Guid.NewGuid(), playerName, playerType, cardDefinitions);
        }

        private void Initialize(Guid runtimePlayerGuid, string playerName, PlayerType playerType, RuntimeCardDefinition[] cardDefinitions)
        {
            _runtimePlayerGuid = runtimePlayerGuid;
            _playerType = playerType;
            _playerName = playerName ?? throw new ArgumentNullException(nameof(playerName));


            // Create a RuntimeCardDefinition Dictionary from RuntimeCardDefinition array.
            _deckCardDefinitions = new Dictionary<Guid, RuntimeCardDefinition>();
            foreach(RuntimeCardDefinition definition in cardDefinitions)
            {
                _deckCardDefinitions.Add(definition.RuntimeCardGuid, definition);
            }


            // Just informs the king os Guid created.
            if(_runtimePlayerGuid == default)
            {
                CardGameTemplate.Debug.LogWarning(Debug.Category.Data, $"{GetType()} created with DEFAULT GUID.");
            }
            else
            {
                CardGameTemplate.Debug.Log(Debug.Category.Data, $"{GetType()} created with Guid: {_runtimePlayerGuid}");
            }
        }
    }
}
