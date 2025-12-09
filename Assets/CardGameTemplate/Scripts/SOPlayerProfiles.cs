using CardGameTemplate;
using UnityEngine;

[CreateAssetMenu(fileName = "SOPlayerProfiles", menuName = "Scriptable Objects/SOPlayerProfiles")]
public class SOPlayerProfiles : ScriptableObject
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