using System;

namespace CardGameTemplate
{
    public class ActionUseCard : IGameAction
    {
        public Guid PlayerGuid { get; private set; }
        public Guid CardGuid { get; private set; }
        public Guid TargetGuid { get; private set;}

        public ActionUseCard(Guid playerGuid, Guid cardGuid, Guid targetGuid = default)
        {
            PlayerGuid = playerGuid;
            CardGuid = cardGuid;
            TargetGuid = targetGuid;
        }
    }

    public class ActionHandlerUseCard : ActionHandler<ActionUseCard>
    {
        private CardGameController _cardGameController;

        public ActionHandlerUseCard(CardGameController cardGameController)
        {
            _cardGameController = cardGameController ?? throw new ArgumentNullException(nameof(cardGameController));
        }

        public override void Execute(ActionUseCard action)
        {
            _cardGameController.TryUseCard(action.PlayerGuid, action.CardGuid, action.TargetGuid);
        }
    }
}
