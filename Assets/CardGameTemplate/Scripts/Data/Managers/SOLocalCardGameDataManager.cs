using UnityEngine;

namespace CardGameTemplate
{
    [CreateAssetMenu(fileName = "NewLocalCardGameDataManager", menuName = "Card Game Template/Local Card Game Data Manager")]
    public class SOLocalCardGameDataManager : ScriptableObject, ICardGameDataManager
    {
        // For local data, the player profiles are stored in scriptable objects. But it can be changed
        // to any other way to store it.
        [SerializeField] private SOPlayerProfilesRegistry _playerProfiles;

        public ICardGameMatchData GetCardGameMatchData()
        {
            return new LocalMatchData(_playerProfiles.GetRuntimePlayerProfiles());
        }
    }
}
