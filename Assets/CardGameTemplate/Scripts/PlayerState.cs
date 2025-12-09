//#define DEBUG_GIZMOS

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CardGameTemplate
{
    /// <summary>
    /// Represents a player's runtime state during a match.  
    /// Stores the player's identity, core components, card sets, and active card behaviours.
    /// <br/>
    /// This class initializes all player-related systems, including health and the
    /// four managed card sets (deck, hand, in-game, discarded). It provides access to
    /// these systems and maintains a unique runtime Guid for identifying the player.
    /// <br/>
    /// In the Unity Editor (when <c>DEBUG_GIZMOS</c> is enabled), this class also draws
    /// debug labels showing the contents of each card set.
    /// </summary>
    public class PlayerState : MonoBehaviour
    {
        private Guid _runtimeGuid;
        private string _playerName;

        // Components
        // TODO: maybe is interesting to store components in a dictionary.
        private ComponentHealth _health;

        [Header("Cards sets")]
        [SerializeField] private CardSetController _deckCards;
        [SerializeField] private CardSetController _handCards;
        [SerializeField] private CardSetController _inGameCards;
        [SerializeField] private CardSetController _discardedCards;

        // Card Effects/Behaviours
        private Dictionary<Guid, ICardBehaviour> _activeCardBehaviours;


        public Guid RuntimePlayerGuid => _runtimeGuid;
        public string PlayerName => _playerName;

        public ComponentHealth ComponentHealth => _health;

        public CardSetController DeckCards => _deckCards;
        public CardSetController HandCards => _handCards;
        public CardSetController InGameCards => _inGameCards;
        public CardSetController DiscardedCards => _discardedCards;


        /// <summary>
        /// Initializes the player's runtime state by assigning identity data,
        /// creating and configuring core components, and instantiating all card sets.<br/>
        ///
        /// This method:<br/>
        /// - Stores the player's runtime Guid and display name.<br/>
        /// - Adds and initializes the <see cref="ComponentHealth"/> component.<br/>
        /// - Creates the four managed card sets (deck, hand, in-game, discarded).<br/>
        /// - Emits a debug log confirming initialization.<br/>
        ///
        /// This must be called once before accessing any player components or card sets.
        /// </summary>
        /// <param name="runtimeGuid">Unique identifier generated for the player at runtime.</param>
        /// <param name="playerName">Player defined nickname.</param>
        public void Initialize(Guid runtimeGuid, string playerName)
        {
            _runtimeGuid = runtimeGuid;
            _playerName = playerName;

            // Add and initialize the components.
            _health = gameObject.AddComponent<ComponentHealth>();
            _health.Initialize(_runtimeGuid, 100, 100);

            // Initialize the card sets.
            _deckCards = new CardSetController(nameof(_deckCards));
            _handCards = new CardSetController(nameof(_handCards));
            _inGameCards = new CardSetController(nameof(_inGameCards));
            _discardedCards = new CardSetController(nameof(_discardedCards));

            Debug.Log(Debug.Category.GameLogic, $"{runtimeGuid} {GetType()} initialized.");
        }

#if UNITY_EDITOR && DEBUG_GIZMOS
        void OnDrawGizmos() 
        {
            DrawCardsLabels(_deckCards, new Vector3(1, 0, 0));
            DrawCardsLabels(_handCards, new Vector3(2, 0, 0));
            DrawCardsLabels(_inGameCards, new Vector3(3, 0, 0));
            DrawCardsLabels(_discardedCards, new Vector3(4, 0, 0));
        }

        void DrawCardsLabels(CardSetController cardSet, Vector3 offset)
        {
            if(cardSet == null) return;

            string text = $"{cardSet.Name}:\n";
            foreach(Guid guid in cardSet.GuidsAsArray)
            {
                text += $"{guid}\n";
            }

            Handles.Label(transform.position + offset, text);
        }
#endif
    }

}
