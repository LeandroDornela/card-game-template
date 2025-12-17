using System;

namespace CardGameTemplate
{
    public class ActionDrawCard : IGameAction
    {
        public Guid PlayerGuid { get; private set; }

        public ActionDrawCard(Guid playerGuid)
        {
            PlayerGuid = playerGuid;
        }
    }

    public class ActionHandlerDrawCard : ActionHandler<ActionDrawCard>
    {
        private CardGameController _cardGameController;

        public ActionHandlerDrawCard(CardGameController cardGameController)
        {
            _cardGameController = cardGameController;
        }

        public override void Execute(ActionDrawCard action)
        {
            _cardGameController.TryDrawACard(action.PlayerGuid);
        }
    }
}
