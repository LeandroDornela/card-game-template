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
        public static Signal<PlayerState> OnNewPlayerAdded = new Signal<PlayerState>();
        public static Signal<float> OnMatchTimerUpdate = new Signal<float>();

        //
        // INPUT
        //
        public static Signal OnKeyPressed_Pause = new Signal();


        //
        // GAME LOGICAL STATES
        //
        public static Signal OnEnterState_Initializing = new Signal();
        public static Signal OnExitState_Initializing = new Signal();
        public static Signal OnEnterState_InGame = new Signal();
        public static Signal OnExitState_InGame = new Signal();
        public static Signal OnEnterState_Paused = new Signal();
        public static Signal OnExitState_Paused = new Signal();
        public static Signal OnEnterState_GameOver = new Signal();
        public static Signal OnExitState_GameOver = new Signal();
        

        //
        // CARD SETS MANIPULATION
        //

        // GENERIC SETS

        public static Signal<Guid, string, RuntimeCardDefinition> OnCardAddedToSet = new Signal<Guid, string, RuntimeCardDefinition>(); // Player Guid, set name, card definition
        public static Signal<Guid, string, Guid> OnCardRemovedFromSet = new Signal<Guid, string, Guid>(); // Player Guid, Set name and card guid
        public static Signal<Guid, string, Guid[]> OnSetCardsOrderChanged = new Signal<Guid, string, Guid[]>(); // Player Guid, Set name and card guids
        
        // FOR SPECIFIC SETS
        // Note: for the UI I judge better to use these since with the generics the UI wold need a way to know the sets names
        // what could be from initialization passim all this data or direct player state access.

        // CARDS MOVED FROM ... TO ...
        public static Signal<Guid, RuntimeCardDefinition> OnCardMoved_DeckToHand = new Signal<Guid, RuntimeCardDefinition>();
        public static Signal<Guid, RuntimeCardDefinition> OnCardMoved_HandToDeck = new Signal<Guid, RuntimeCardDefinition>();
        public static Signal<Guid, RuntimeCardDefinition> OnCardMoved_HandToDiscarded = new Signal<Guid, RuntimeCardDefinition>();
        public static Signal<Guid, RuntimeCardDefinition> OnCardMoved_DiscardedToHand = new Signal<Guid, RuntimeCardDefinition>();
        public static Signal<Guid, RuntimeCardDefinition> OnCardMoved_DeckToDiscarded = new Signal<Guid, RuntimeCardDefinition>();
        public static Signal<Guid, RuntimeCardDefinition> OnCardMoved_DiscardedToDeck = new Signal<Guid, RuntimeCardDefinition>();
        // DECK
        public static Signal<Guid, RuntimeCardDefinition> OnCardAdded_Deck = new Signal<Guid, RuntimeCardDefinition>(); // Player Guid, card definition
        public static Signal<Guid, RuntimeCardDefinition> OnCardRemoved_Deck = new Signal<Guid, RuntimeCardDefinition>(); // Player Guid, card definition
        // HAND
        public static Signal<Guid, RuntimeCardDefinition> OnCardAdded_Hand = new Signal<Guid, RuntimeCardDefinition>(); // Player Guid, card definition
        public static Signal<Guid, RuntimeCardDefinition> OnCardRemoved_Hand = new Signal<Guid, RuntimeCardDefinition>(); // Player Guid, card definition
        // DISCARDED
        public static Signal<Guid, RuntimeCardDefinition> OnCardAdded_Discarded = new Signal<Guid, RuntimeCardDefinition>(); // Player Guid, card definition
        public static Signal<Guid, RuntimeCardDefinition> OnCardRemoved_Discarded = new Signal<Guid, RuntimeCardDefinition>(); // Player Guid, card definition

        //
        // COMPONENTS UPDATE
        //

        // Player Guid, new value
        public static Signal<Guid, float> OnPlayerHealthChanged = new Signal<Guid, float>();
        public static Signal<Guid> OnPlayerHealthIsZero = new Signal<Guid>();
    }
}
