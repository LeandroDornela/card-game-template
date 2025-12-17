using System;

namespace CardGameTemplate
{
    public interface IActionHandler
    {
        Type ActionType { get; }

        void Execute(IGameAction action);
    }
}
