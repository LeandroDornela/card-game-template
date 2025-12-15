using UnityEngine;

namespace CardGameTemplate
{
    public class StatePaused : IGameState
    {
        private CardGameManager _cardGameManager;
        private bool _pauseKeyPressed = false;

        public InGameStateId StateId => InGameStateId.Paused;

        public StatePaused(CardGameManager cardGameManager)
        {
            _cardGameManager = cardGameManager;
        }

        public void OnEnter(InGameStateId lastState)
        {
            Debug.Log(Debug.Category.GameState, $"Entering {GetType()}.");

            _pauseKeyPressed = false;

            // Listen to a pause game input.
            GameEvents.OnKeyPressed_Pause.AddListener(HandleInput);

            Time.timeScale = 0;

            GameEvents.OnEnterState_Paused.Trigger();
        }

        public InGameStateId OnUpdate()
        {
            if(_pauseKeyPressed)
            {
                return InGameStateId.InGame;
            }

            return InGameStateId.None;
        }

        public void OnExit(InGameStateId nextState)
        {
            Debug.Log(Debug.Category.GameState, $"Exiting {GetType()}.");

            // Remember always remove listeners.
            GameEvents.OnKeyPressed_Pause.RemoveListener(HandleInput);

            Time.timeScale = 1;

            GameEvents.OnExitState_Paused.Trigger();
        }


        void HandleInput()
        {
            _pauseKeyPressed = true;
        }
    }
}
