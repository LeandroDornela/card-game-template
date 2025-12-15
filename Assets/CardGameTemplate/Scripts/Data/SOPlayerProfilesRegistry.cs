using CardGameTemplate;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerProfiles", menuName = "Card Game Template/Player Profiles Registry")]
public class SOPlayerProfilesRegistry : ScriptableObject
{
    public SOPlayerProfile[] PlayerProfiles;

    public RuntimePlayerProfile GetRuntimePlayerProfile(int playerIndex)
    {
        return PlayerProfiles[playerIndex].GetRuntimePlayerProfile();
    }

    public RuntimePlayerProfile[] GetRuntimePlayerProfiles()
    {
        RuntimePlayerProfile[] result = new RuntimePlayerProfile[PlayerProfiles.Length];

        for(int i = 0; i < result.Length; i++)
        {
            result[i] = PlayerProfiles[i].GetRuntimePlayerProfile();
        }

        return result;
    }
}