using CardGameTemplate;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "NewControllerDefinitionsRegistry", menuName = "Card Game Template/Controller Definitions Registry")]
public class SOControllerDefinitionsRegistry : ScriptableObject
{
    [SerializeField] private SerializedDictionary<PlayerType, GameObject> _controllerPrefabDefinitions;

    public bool TryGetPrefab(PlayerType playerType, out GameObject prefab)
    {
        if(!_controllerPrefabDefinitions.TryGetValue(playerType, out prefab))
        {
            return false;
        }

        return true;
    }
}
