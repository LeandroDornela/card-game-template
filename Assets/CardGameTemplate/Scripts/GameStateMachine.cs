using System.Collections.Generic;

namespace CardGameTemplate
{
    /// <summary>
    /// The States of the State Machine control WHAT should happen on each state. Not HOW.
    /// The how is defined by the classes called by the State.<br/>
    /// <br/>
    /// NOTE: States can transition based on internal logic or external game conditions.
    /// When the condition is external (e.g., a game-over event), the state should
    /// not be accessed directly. Instead, the state should subscribe to events or
    /// register callbacks so it can react when those triggers occur.<br/>
    /// This ensures that the rest of the game code does not need to reference the
    /// state machine directly, reducing coupling and improving modularity.
    /// </summary>
    [System.Serializable]
    public class GameStateMachine
    {
        private readonly Dictionary<InGameStateId, IGameState> _states;
        private IGameState _current;
        private IGameState _last;


        public InGameStateId CurrentState => _current.StateId;
        public InGameStateId LastState => _last.StateId;


        public GameStateMachine(Dictionary<InGameStateId, IGameState> states)
        {
            _states = states;
        }


        /// <summary>
        /// Start the state machine by setting the Current and Last states as the startingState
        ///  and calls Enter method of Current State.
        /// </summary>
        /// <param name="startingState">Initial state of the state machine.</param>
        public void StartMachine(InGameStateId startingState)
        {
            _current = _states[startingState];
            _last = _current;
            _current.OnEnter(_last.StateId);
        }


        public void Update()
        {
            InGameStateId newState = _current.OnUpdate();

            if(newState != InGameStateId.None)
            {
                ChangeState(newState);
            }
        }


        /// <summary>
        /// Set a new Current and Last state.
        /// NOTE: Private change state to force change logic live inside the states and manager.
        /// </summary>
        /// <param name="newState">New Current state.</param>
        void ChangeState(InGameStateId newState)
        {
            _current.OnExit(newState);
            _last = _current;
            _current = _states[newState];
            _current.OnEnter(_last.StateId);
        }
    }
}
