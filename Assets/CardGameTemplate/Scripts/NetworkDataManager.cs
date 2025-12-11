using System;
using UnityEngine;

namespace CardGameTemplate
{
    public class NetworkDataManager : MonoBehaviour, ICardGameDataManager
    {
        public ICardGameMatchData GetCardGameMatchData()
        {
            throw new NotImplementedException();
        }
    }
}