namespace CardGameTemplate
{
    public interface IGameState
    {
        InGameStateId StateId {get;}

        void OnEnter(InGameStateId lastState);
        void OnExit(InGameStateId nextState);
        InGameStateId OnUpdate();
    }
}
