namespace CardGameTemplate
{
    public class StateGameOver : IGameState
    {
        private CardGameManager _cardGameManager;

        public InGameStateId StateId => InGameStateId.GameOver;

        public StateGameOver(CardGameManager cardGameManager)
        {
            _cardGameManager = cardGameManager;
        }

        public void OnEnter(InGameStateId lastState)
        {
            Debug.Log(Debug.Category.GameState, $"Entering {GetType()}.");

            GameEvents.OnEnterState_GameOver.Trigger();
        }

        public InGameStateId OnUpdate()
        {
            return InGameStateId.None;
        }

        public void OnExit(InGameStateId nextState)
        {
            Debug.Log(Debug.Category.GameState, $"Exiting {GetType()}.");
            
            GameEvents.OnExitState_GameOver.Trigger();
        }
    }
}
