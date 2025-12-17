using System;

namespace CardGameTemplate
{
    // Generalization for the ActionHandler ONLY to ensure type safety. ActionType and Execute input must be the same type.
    public abstract class ActionHandler<T> : IActionHandler where T : IGameAction
    {
        Type IActionHandler.ActionType => typeof(T);

        // This will be called by ActionHandlersManager since the handles are store as IActionHandler
        // Then it will call the abstract Execute(T action) implemented in derived classes.
        // All this just to support generic types and have type safety.
        void IActionHandler.Execute(IGameAction action)
        {
            Debug.Log(Debug.Category.GameLogic, $"Executing action {action.GetType()}.");
            Execute((T)action);
        }

        public abstract void Execute(T action);
    }
}
