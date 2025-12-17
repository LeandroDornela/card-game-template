using System;

namespace CardGameTemplate
{
    public interface IGameAction
    {
        Guid PlayerGuid { get; }
    }
}
