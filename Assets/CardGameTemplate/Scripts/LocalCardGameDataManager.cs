using System;
using UnityEngine;

namespace CardGameTemplate
{
    [System.Serializable]
    public class LocalCardGameDataManager : ICardGameDataManager
    {
        [SerializeField] private SOPlayerProfiles _playerProfiles;

        public override ICardGameMatchData GetCardGameMatchData()
        {
            _cardGameMatchData = new ICardGameMatchData(_playerProfiles.GetRuntimePlayerProfiles());

            return _cardGameMatchData;
        }
    }
}
