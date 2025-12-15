namespace CardGameTemplate
{
    public class LocalMatchData : ICardGameMatchData
    {
        public RuntimePlayerProfile[] RuntimePlayerProfiles { get; private set; }

        public LocalMatchData(RuntimePlayerProfile[] playerProfiles)
        {
            RuntimePlayerProfiles = playerProfiles;
        }
    }
}
