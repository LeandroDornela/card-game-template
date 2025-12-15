using System;
using System.Collections.Generic;

namespace CardGameTemplate
{
    /// <summary>
    /// Represents a player's runtime state during a match.  
    /// Stores the player's identity, core components, card sets, and active card behaviours.
    /// <br/>
    /// This class initializes all player-related systems, including health and the
    /// four managed card sets (deck, hand, in-game, discarded). It provides access to
    /// these systems and maintains a unique runtime Guid for identifying the player.
    /// </summary>
    public class PlayerState
    {
        private Guid _runtimeGuid;
        private string _playerName;
        // Components
        private Dictionary<PlayerComponent, IComponent> _components;
        // Cards sets
        private Dictionary<CardSetId, CardSetController> _cardSets;
        // Active Card Effects/Behaviours
        private Dictionary<Guid, CardBehaviour> _activeCardBehaviours;


        public Guid RuntimePlayerGuid => _runtimeGuid;
        public string PlayerName => _playerName;
        public PlayerFloatComponent Health => (PlayerFloatComponent)_components[PlayerComponent.Health];


        public bool TryGetCardSetController(CardSetId cardSetId, out CardSetController cardSetController)
        {
            if(_cardSets == null)
            {
                Debug.LogError(Debug.Category.Data, "Card sets dictionary is not initialized.");
                cardSetController = default;
                return false;
            }

            if(_cardSets.Count == 0)
            {
                Debug.LogError(Debug.Category.Data, "Card sets dictionary have 0 elements.");
                cardSetController = default;
                return false;
            }

            if(!_cardSets.TryGetValue(cardSetId, out cardSetController))
            {
                Debug.LogError(Debug.Category.Data, $"Key {cardSetId} don't found on Card sets dictionary.");
                return false;
            }

            return true;
        }


        /// <summary>
        /// Initializes the player's runtime state by assigning identity data,
        /// creating and configuring core components, and instantiating all card sets.<br/>
        ///
        /// This method:<br/>
        /// - Stores the player's runtime Guid and display name.<br/>
        /// - Adds and initializes the <see cref="Health"/> component.<br/>
        /// - Creates the managed card sets (deck, hand, in-game, discarded).<br/>
        /// </summary>
        /// <param name="runtimeGuid">Unique identifier generated for the player at runtime.</param>
        /// <param name="playerName">Player defined nickname.</param>
        public PlayerState(Guid runtimeGuid, string playerName, PlayerComponentsData defaultComponentsStats)
        {
            _runtimeGuid = runtimeGuid;
            _playerName = playerName;

            // Create health component and register to value change events.
            PlayerFloatComponent newCompHealth = new PlayerFloatComponent
            (
                _runtimeGuid,
                defaultComponentsStats.CurrentHealth,
                0,
                defaultComponentsStats.MaxHealth
            );
            newCompHealth.OnValueChanged.AddListener(OnPlayerHealthChanged);
            newCompHealth.OnValueIsMin.AddListener(OnPlayerHealthIsZero);

            // Create mana component.
            PlayerFloatComponent newCompMana = new PlayerFloatComponent
            (
                _runtimeGuid,
                defaultComponentsStats.CurrentMana,
                0,
                defaultComponentsStats.MaxMana
            );

            // Add and initialize the components.
            _components = new Dictionary<PlayerComponent, IComponent>
            {
                {PlayerComponent.Health, newCompHealth},
                {PlayerComponent.Mana, newCompMana}
            };

            // Initialize the card sets.
            _cardSets = new Dictionary<CardSetId, CardSetController>
            {
                {CardSetId.Deck, new CardSetController(CardSetId.Deck)},
                {CardSetId.Hand, new CardSetController(CardSetId.Hand)},
                {CardSetId.InGame, new CardSetController(CardSetId.InGame)},
                {CardSetId.Discarded, new CardSetController(CardSetId.Discarded)},
            };

            Debug.Log(Debug.Category.GameLogic, $"{runtimeGuid} {GetType()} initialized.");
        }


        void OnPlayerHealthChanged(Guid ownerGuid, float oldValue, float newValue)
        {
            GameEvents.OnPlayerHealthChanged.Trigger(ownerGuid, newValue);
        }


        void OnPlayerHealthIsZero(Guid ownerGuid, float oldValue, float newValue)
        {
            GameEvents.OnPlayerHealthIsZero.Trigger(ownerGuid);
        }
    }
}
