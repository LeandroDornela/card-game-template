using UnityEngine;

namespace CardGameTemplate
{
    [System.Serializable]
    public abstract class ICardGameDataManager
    {
        protected ICardGameMatchData _cardGameMatchData;
        public abstract ICardGameMatchData GetCardGameMatchData();
    }
}