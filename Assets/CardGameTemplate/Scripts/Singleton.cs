using UnityEngine;

namespace CardGameTemplate
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;
        public static T Instance
        {
            // TODO: ler sobre lock()
            get
            {
                CheckInstance();
                return _instance;
            }
        }

        private static void CheckInstance()
        {
            // Check if the instance is null
            if (_instance == null)
            {
                // Find the existing instance in the scene
                _instance = FindAnyObjectByType<T>();

                if (_instance == null)
                {
                    CardGameTemplate.Debug.LogError(Debug.Category.GameLogic, "Instance not found in scene!");
                }
            }
        }
    }
}
