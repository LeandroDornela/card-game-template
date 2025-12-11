namespace CardGameTemplate
{
    public enum BehaviourType
    {
        DamageHealth
        //RecoverHealth,
        //Combat
    }

    public enum PlayerType
    {
        LocalHuman,
        LocalAI
    }

    public enum PlayerComponent
    {
        Health,
        Mana
    }

    public enum TargetType
    {
        OwnerPlayer,
        EnemyPlayers
        //EnemyCard
    }

    public enum InGameStateId
    {
        None,
        Initializing,
        InGame,
        Paused,
        GameOver
    }
}