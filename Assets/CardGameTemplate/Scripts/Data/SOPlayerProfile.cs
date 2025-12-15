using UnityEngine;

namespace CardGameTemplate
{
    [CreateAssetMenu(fileName = "NewPlayerProfile", menuName = "Card Game Template/Player Profile")]
    public class SOPlayerProfile : ScriptableObject
    {
        [SerializeField] private string _playerName;
        [SerializeField] private PlayerType _playerType;
        [SerializeField] private PlayerComponentsData _defaultComponentsStats;
        [SerializeField] private SOCardDefinition[] _SOCardDefinitions;


        public RuntimePlayerProfile GetRuntimePlayerProfile()
        {
            // Validate data.
            if(_SOCardDefinitions == null || _SOCardDefinitions.Length == 0)
            {
                Debug.LogError(Debug.Category.Data, $"{nameof(_SOCardDefinitions)} is invalid.");
                return default;
            }

            if(_playerName == default)
            {
                Debug.LogError(Debug.Category.Data, $"{nameof(_playerName)} is invalid.");
                return default;
            }
            
            //
            // Runtime player profile creation.
            //

            RuntimeCardDefinition[] rtCardDefinitions = new RuntimeCardDefinition[_SOCardDefinitions.Length];
            
            // For each CARD Definition, try create a RUNTIME Card Definition.
            for(int i = 0; i < _SOCardDefinitions.Length; i++)
            {
                if(!RuntimeCardFactory.TryCreateRuntimeCardDefinition(_SOCardDefinitions[i].CardDefinition, out rtCardDefinitions[i]))
                {
                    Debug.LogError(Debug.Category.Data, $"Unable to create card definition for {_SOCardDefinitions[i].CardDefinition.CardName}");
                    return default;
                }
            }

            return new RuntimePlayerProfile(_playerName, _playerType, _defaultComponentsStats, rtCardDefinitions);
        }
    }
}
