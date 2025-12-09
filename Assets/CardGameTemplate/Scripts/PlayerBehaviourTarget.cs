namespace CardGameTemplate
{
    public class PlayerBehaviourTarget : IBehaviourTargetWrapper
    {
        private PlayerState _player; // Or who has the components


        public PlayerBehaviourTarget(PlayerState player)
        {
            _player = player;
        }


        public T GetSubTargetsHolder<T>() where T : class
        {
            if(typeof(T) != typeof(PlayerState))
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"Invalid type [{typeof(T)}]. It must be [{typeof(PlayerState)}]");
                return default;
            }

            return (T)(object)_player;
        }
    }
}
