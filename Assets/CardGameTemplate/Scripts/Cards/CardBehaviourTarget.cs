namespace CardGameTemplate
{
    public class CardBehaviourTarget : IBehaviourTargetWrapper
    {
        private RuntimeCardDefinition _card; // Who has the effects.

        public CardBehaviourTarget(RuntimeCardDefinition card)
        {
            _card = card;
        }


        public T GetSubTargetsHolder<T>() where T : class
        {
            if(typeof(T) != typeof(RuntimeCardDefinition))
            {
                Debug.LogError(Debug.Category.Data, $"Invalid type [{typeof(T)}]. It must be [{typeof(RuntimeCardDefinition)}]");
                return default;
            }

            return (T)(object)_card;
        }
    }
}