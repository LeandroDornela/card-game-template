namespace CardGameTemplate
{
    /// <summary>
    /// Represents the active gameplay state of the match.  
    /// Handles initialization logic when first entering gameplay,  
    /// listens for pause input, advances the match timer, and  
    /// transitions to the Paused state when necessary.  
    /// Also ensures proper event subscription and cleanup when  
    /// entering or exiting the state.
    /// </summary>
    public class StateInGame : IGameState
    {
        private CardGameManager _cardGameManager;
        private bool _pauseKeyPressed = false;

        public InGameStateId StateId => InGameStateId.InGame;


        public StateInGame(CardGameManager cardGameManager)
        {
            _cardGameManager = cardGameManager;
        }


        public void OnEnter(InGameStateId lastState)
        {
            Debug.Log(Debug.Category.GameState, $"Entering {GetType()}.");

            _pauseKeyPressed = false;

            // Listen to a pause game input.
            GameEvents.OnKeyPressed_Pause.AddListener(HandleInput);

            // Is first time entering in game.
            // TODO: this could be a new state between initialization and in game.
            if(lastState == InGameStateId.Initializing)
            {
                _cardGameManager.CardGameController.FillPlayersDecks();
                _cardGameManager.CardGameController.ShufflePlayersDecks();
                _cardGameManager.CardGameController.TryFillPlayersHands();
            }

            GameEvents.OnEnterState_InGame.Trigger();
        }


        public InGameStateId OnUpdate()
        {
            if(_pauseKeyPressed)
            {
                return InGameStateId.Paused;
            }

            // Only update the match timer in game.
            _cardGameManager.MatchState.TickTimer();

            return InGameStateId.None;
        }


        public void OnExit(InGameStateId nextState)
        {
            Debug.Log(Debug.Category.GameState, $"Exiting {GetType()}.");

            // Remember always remove listeners.
            GameEvents.OnKeyPressed_Pause.RemoveListener(HandleInput);

            GameEvents.OnExitState_InGame.Trigger();
        }
        

        void HandleInput()
        {
            _pauseKeyPressed = true;
        }
    }
}
