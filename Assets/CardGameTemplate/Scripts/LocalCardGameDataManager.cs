using System;
using UnityEngine;

namespace CardGameTemplate
{
    [CreateAssetMenu(fileName = "LocalCardGameDataManager", menuName = "Scriptable Objects/LocalCardGameDataManager")]
    public class LocalCardGameDataManager : ScriptableObject, ICardGameDataManager
    {
        // For local data, the player profiles are stored in scriptable objects. But it can be changed
        // to any other way to store it.
        [SerializeField] private SOPlayerProfiles _playerProfiles;

        public ICardGameMatchData GetCardGameMatchData()
        {
            return new LocalMatchData(_playerProfiles.GetRuntimePlayerProfiles());
        }
    }
}
