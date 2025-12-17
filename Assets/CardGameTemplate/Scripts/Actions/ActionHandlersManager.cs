using System;
using System.Collections.Generic;

namespace CardGameTemplate
{
    public class ActionHandlersManager
    {
        // Ensure all "object" type values are IActionHandler at "Add" moment.
        private readonly Dictionary<Type, IActionHandler> _actionHandlers = new();

        // Will register a new object that is a IActionHandler and the generic type of IActionHandler must be a IGameAction
        public void Add(IActionHandler actionHandler)
        {
            _actionHandlers.Add(actionHandler.ActionType, actionHandler);
        }


        public void HandleActionRequest(IGameAction action)
        {
            if(_actionHandlers.TryGetValue(action.GetType(), out IActionHandler actionHandler))
            {
                actionHandler.Execute(action);
            }
            else
            {
                Debug.Log(Debug.Category.GameLogic, $"Can't find Action Handler for {action.GetType()}");
            }
        }
    }
}
