using UnityEngine;
using System;

namespace CardGameTemplate
{   
    /// <summary>
    /// Centralized collection of global game signals used for communication between
    /// gameplay systems, UI, and state logic.<br/> 
    /// <br/>
    /// This class exposes strongly typed <see cref="Signal"/> instances grouped into:<br/>
    /// <br/>
    /// -Player Events: such as adding new players.<br/>
    /// -Input Events: game input actions (e.g., pause key).<br/>
    /// -Game State Events: entering and exiting logical gameplay states.<br/>
    /// -Card Set Manipulation Events: adding, removing, and moving cards between sets.<br/>
    /// -Component Update Events: changes in player components (e.g., health).<br/>
    /// <br/>
    /// All events follow a publish/subscribe pattern, allowing systems to react to game
    /// changes without direct dependencies, reducing coupling and improving modularity.<br/>
    /// <br/>
    /// NOTE: Always remember to UNSUBSCRIBE from the signals when deactivating or destroying
    /// a object associated with a subscribed Action.<br/>
    /// </summary>

    public static class GameEvents
    {
        public static Signal<PlayerState> OnNewPlayerAdded = new();
        public static Signal<float> OnMatchTimerUpdate = new();
        public static Signal<IGameAction> OnGameActionRequest = new(); // Generic Action request.

        //
        // INPUT
        //
        public static Signal OnKeyPressed_Pause = new();


        //
        // GAME LOGICAL STATES
        //
        public static Signal OnEnterState_Initializing = new();
        public static Signal OnExitState_Initializing = new();
        public static Signal OnEnterState_InGame = new();
        public static Signal OnExitState_InGame = new();
        public static Signal OnEnterState_Paused = new();
        public static Signal OnExitState_Paused = new();
        public static Signal OnEnterState_GameOver = new();
        public static Signal OnExitState_GameOver = new();
        

        //
        // CARD SETS MANIPULATION
        //

        // GENERIC SETS

        public static Signal<Guid, string, RuntimeCardDefinition> OnCardAddedToSet = new(); // Player Guid, set name, card definition
        public static Signal<Guid, string, Guid> OnCardRemovedFromSet = new(); // Player Guid, Set name and card guid
        public static Signal<Guid, string, Guid[]> OnSetCardsOrderChanged = new(); // Player Guid, Set name and card guids
        
        // FOR SPECIFIC SETS
        // Note: for the UI I judge better to use these since with the generics the UI wold need a way to know the sets names
        // what could be from initialization passim all this data or direct player state access.

        // CARDS MOVED FROM ... TO ...
        public static Signal<Guid, RuntimeCardDefinition> OnCardMoved_DeckToHand = new();
        public static Signal<Guid, RuntimeCardDefinition> OnCardMoved_HandToDeck = new();
        public static Signal<Guid, RuntimeCardDefinition> OnCardMoved_HandToDiscarded = new();
        public static Signal<Guid, RuntimeCardDefinition> OnCardMoved_DiscardedToHand = new();
        public static Signal<Guid, RuntimeCardDefinition> OnCardMoved_DeckToDiscarded = new();
        public static Signal<Guid, RuntimeCardDefinition> OnCardMoved_DiscardedToDeck = new();
        // DECK
        public static Signal<Guid, RuntimeCardDefinition> OnCardAdded_Deck = new(); // Player Guid, card definition
        public static Signal<Guid, RuntimeCardDefinition> OnCardRemoved_Deck = new(); // Player Guid, card definition
        // HAND
        public static Signal<Guid, RuntimeCardDefinition> OnCardAdded_Hand = new(); // Player Guid, card definition
        public static Signal<Guid, RuntimeCardDefinition> OnCardRemoved_Hand = new(); // Player Guid, card definition
        // DISCARDED
        public static Signal<Guid, RuntimeCardDefinition> OnCardAdded_Discarded = new(); // Player Guid, card definition
        public static Signal<Guid, RuntimeCardDefinition> OnCardRemoved_Discarded = new(); // Player Guid, card definition

        //
        // COMPONENTS UPDATE
        //

        // Player Guid, new value
        public static Signal<Guid, float> OnPlayerHealthChanged = new();
        public static Signal<Guid> OnPlayerHealthIsZero = new();
    }
}
