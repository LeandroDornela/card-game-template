using System;
using System.Collections.Generic;

namespace CardGameTemplate
{
    public class RuntimePlayerProfile
    {
        private Dictionary<Guid, RuntimeCardDefinition> _deckCardDefinitions;

        public Guid RuntimePlayerGuid {get; private set; }
        public string PlayerName {get; private set; }
        public PlayerType PlayerType {get; private set; }
        public PlayerComponentsData DefaultComponentsStats {get; private set; }
        public IReadOnlyDictionary<Guid, RuntimeCardDefinition> DeckCardDefinitions => _deckCardDefinitions;


        public RuntimePlayerProfile(string playerName, PlayerType playerType, PlayerComponentsData defaultComponentsStats, RuntimeCardDefinition[] cardDefinitions)
        {
            Initialize(Guid.NewGuid(), playerName, playerType, defaultComponentsStats, cardDefinitions);
        }


        private void Initialize(Guid runtimePlayerGuid, string playerName, PlayerType playerType, PlayerComponentsData defaultComponentsStats, RuntimeCardDefinition[] cardDefinitions)
        {
            RuntimePlayerGuid = runtimePlayerGuid;
            PlayerType = playerType;
            PlayerName = playerName ?? throw new ArgumentNullException(nameof(playerName));
            DefaultComponentsStats = defaultComponentsStats;


            // Create a RuntimeCardDefinition Dictionary from RuntimeCardDefinition array.
            _deckCardDefinitions = new Dictionary<Guid, RuntimeCardDefinition>();
            foreach(RuntimeCardDefinition definition in cardDefinitions)
            {
                _deckCardDefinitions.Add(definition.RuntimeCardGuid, definition);
            }


            // Just informs the king os Guid created.
            if(RuntimePlayerGuid == default)
            {
                Debug.LogWarning(Debug.Category.Data, $"{GetType()} created with DEFAULT GUID.");
            }
            else
            {
                Debug.Log(Debug.Category.Data, $"{GetType()} created with Guid: {RuntimePlayerGuid}");
            }
        }
    }
}
