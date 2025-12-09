using System;
using System.IO;
using UnityEngine;

namespace CardGameTemplate
{
    [CreateAssetMenu(fileName = "SOPlayerProfile", menuName = "Scriptable Objects/SOPlayerProfile")]
    public class SOPlayerProfile : ScriptableObject
    {
        [SerializeField] private string _playerName;
        [SerializeField] private PlayerType _playerType;
        [SerializeField] private SOCardDefinition[] _SOCardDefinitions;


        // TODO: Inherit from interface and implement the method to return the definition.
        public RuntimePlayerProfile GetRuntimePlayerProfile()
        {
            // Validate data.
            if(_SOCardDefinitions == null || _SOCardDefinitions.Length == 0)
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"{nameof(_SOCardDefinitions)} is invalid.");
                return default;
            }

            if(_playerName == default)
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"{nameof(_playerName)} is invalid.");
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
                    CardGameTemplate.Debug.LogError(Debug.Category.Data, $"Unable to create card definition for {_SOCardDefinitions[i].CardDefinition.CardName}");
                    return default;
                }
            }

            return new RuntimePlayerProfile(_playerName, _playerType, rtCardDefinitions);
        }

// TODO: Review of remove
#region  AI GENERATED CODE
        [NaughtyAttributes.Button]
        [Obsolete]
        public void SaveAsJSON()
        {
            // Validate data before serializing
            if (_SOCardDefinitions == null || _SOCardDefinitions.Length == 0)
            {
                Debug.LogError(Debug.Category.Data, $"{nameof(_SOCardDefinitions)} is invalid.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_playerName))
            {
                Debug.LogError(Debug.Category.Data, $"{nameof(_playerName)} is invalid.");
                return;
            }

            // Build a serialisable object that contains everything we need.
            var serialisableProfile = new SerializablePlayerProfile
            {
                PlayerName   = _playerName,
                PlayerType   = _playerType,
                CardDefs     = new SerializableCardDefinition[_SOCardDefinitions.Length]
            };

            for (int i = 0; i < _SOCardDefinitions.Length; i++)
            {
                var cardDef = _SOCardDefinitions[i].CardDefinition;
                serialisableProfile.CardDefs[i] = new SerializableCardDefinition
                {
                    CardName   = cardDef.CardName,
                    Description= cardDef.Description,
                    Effects    = cardDef.Effects
                };
            }

            // Convert to JSON (pretty‑printed for readability)
            string json = JsonUtility.ToJson(serialisableProfile, true);

            // Decide where to write the file.
            // We place it one level above Assets so that it is not part of the Unity project assets.
            string folderPath = Path.Combine(Application.dataPath, "..", "PlayerProfiles");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName   = $"{_playerName}.json";
            string fullPath   = Path.Combine(folderPath, fileName);

            try
            {
                File.WriteAllText(fullPath, json);
                Debug.Log(Debug.Category.Data, $"Profile saved to: {fullPath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError(Debug.Category.Data, $"Failed to write profile JSON. Exception: {ex.Message}");
            }
        }

        // Helper structs used only for serialisation – they are not exposed as ScriptableObjects.
        struct SerializablePlayerProfile
        {
            public string PlayerName;
            public PlayerType PlayerType;
            public SerializableCardDefinition[] CardDefs;
        }
        [System.Serializable]
        struct SerializableCardDefinition
        {
            public string CardName;
            public string Description;
            public BehaviourDefinition[] Effects;   // EffectDefinition is already serialisable
        }
#endregion
    }
}
