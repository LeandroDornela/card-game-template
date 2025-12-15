using UnityEngine;

namespace CardGameTemplate
{
    [CreateAssetMenu(fileName = "CardGameConfig", menuName = "Card Game Template/Card Game Config")]
    public class CardGameConfig : ScriptableObject
    {
        [SerializeField] private Object _cardGameDataManager;
        [SerializeField] private SOControllerDefinitionsRegistry _controllerPrefabsDefinitions;
        [SerializeField] private int _maxCardsInHand = 5;

        // _cardGameDataManager as ICardGameDataManager = can return null
        // (ICardGameDataManager)_cardGameDataManager = will thrown exception, use to force interface implementation and do't accept nulls
        public ICardGameDataManager CardGameDataManager => (ICardGameDataManager)_cardGameDataManager;
        public SOControllerDefinitionsRegistry ControllerPrefabsDefinitions => _controllerPrefabsDefinitions;
        public int MaxCardsInHand => _maxCardsInHand;


        void OnValidate()
        {
            bool isValid = false;

            if(_cardGameDataManager is ICardGameDataManager)
            {
                isValid = true;
            }
            else if(_cardGameDataManager is GameObject gameObject)
            {
                isValid = gameObject.TryGetComponent<ICardGameDataManager>(out _);
            }

            if(!isValid)
            {
                Debug.LogError(Debug.Category.Data, $"The assigned assets to {nameof(_cardGameDataManager)} is not a {nameof(ICardGameDataManager)} implementation.");
                _cardGameDataManager = null;
            }
        }
    }
}
