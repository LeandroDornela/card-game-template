using UnityEngine;

namespace CardGameTemplate
{
    public class StateInitializing : IGameState
    {
        private CardGameManager _cardGameManager;
        private bool _initializationFinished = false; // For async initialization.

        public InGameStateId StateId => InGameStateId.Initializing;


        public StateInitializing(CardGameManager cardGameManager)
        {
            _cardGameManager = cardGameManager;
        }


        public void OnEnter(InGameStateId lastState)
        {
            CardGameTemplate.Debug.Log(Debug.Category.GameState, $"Entering {GetType()}.");

            // The game manager will get the match data and instantiate the initial players.
            _cardGameManager.SetupPlayersContext(() => _initializationFinished = true);

            GameEvents.OnEnterState_Initializing.Trigger();
        }


        public InGameStateId OnUpdate()
        {
            // The initialization can take a while if the data came from remote source. Wait until
            // the initialization is complete.
            if(_initializationFinished)
            {
                return InGameStateId.InGame;
            }
            
            return InGameStateId.None;
        }
        

        public void OnExit(InGameStateId nextState)
        {
            CardGameTemplate.Debug.Log(Debug.Category.GameState, $"Exiting {GetType()}.");

            GameEvents.OnExitState_Initializing.Trigger();
        }
    }
}
