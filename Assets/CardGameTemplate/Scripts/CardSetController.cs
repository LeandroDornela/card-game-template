using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardGameTemplate
{
    /// <summary>
    /// Wrapper for storing and manage a set os cards and hide the data structure behind it. 
    /// </summary>
    [System.Serializable]
    public class CardSetController
    {
        private string _setName;
        private Dictionary<Guid, RuntimeCardDefinition> _cards; // Main structure.
        private List<Guid> _guids; // For cards order.
        private bool _initialized = false;

#if UNITY_EDITOR
        [SerializeField, NaughtyAttributes.ResizableTextArea] private string _debugGuids;
#endif

        /// <summary>
        /// Return the card set Name, received at the constructor.
        /// </summary>
        public string Name => _setName;

        private IReadOnlyDictionary<Guid, RuntimeCardDefinition> Cards => _cards; // Main structure.
        /// <summary>
        /// Return the cards as a array. Expensive, convert original structure to an array.
        /// </summary>
        public RuntimeCardDefinition[] CardsAsArray => _cards.Values.ToArray();
        /// <summary>
        /// Return the cards as a list. Expensive, convert original structure to an list.
        /// </summary>
        public List<RuntimeCardDefinition> CardsAsList => _cards.Values.ToList();

        /// <summary>
        /// Return the guids as a array. Expensive, convert original structure to an array.
        /// </summary>
        public Guid[] GuidsAsArray => _guids.ToArray();
        /// <summary>
        /// Return the guids as a list. Expensive, convert original structure to an new list.
        /// </summary>
        public List<Guid> GuidsAsList => new List<Guid>(_guids);

        /// <summary>
        /// Return the total number of cards in the Set.
        /// </summary>
        public int Count => _guids.Count;


        public CardSetController(string setName)
        {
            _setName = setName;
            _cards = new Dictionary<Guid, RuntimeCardDefinition>();
            _guids = new List<Guid>();
            
            _initialized = true;
        }


        public CardSetController(string setName, Guid[] guids, RuntimeCardDefinition[] cards)
        {
            _setName = setName;
            _cards = new Dictionary<Guid, RuntimeCardDefinition>();
            _guids = new List<Guid>();

            for(int i = 0; i < guids.Length; i++)
            {
                _cards.Add(guids[i], cards[i]); // Cards are referenced, not copied.
                _guids.Add(guids[i]);
            }

#if UNITY_EDITOR
            UpdateDebugGuids();
#endif

            _initialized = true;
        }


        public CardSetController(string setName, Dictionary<Guid, RuntimeCardDefinition> cards)
        {
            _setName = setName;

            Overwrite(cards);

            _initialized = true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsInitialized()
        {
            if(!_initialized || _cards == null || _guids == null)
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"{_setName}: Card set is not initialized.");
                return false;
            }

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsEmpty()
        {
            if(_cards?.Count == 0)
            {
                CardGameTemplate.Debug.LogWarning(Debug.Category.Data, $"{_setName}: Card Set is empty.");
                return true;
            }

            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cards"></param>
        public void Overwrite(IReadOnlyDictionary<Guid, RuntimeCardDefinition> cards)
        {
            if(cards == null)
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"{_setName}: Invalid input parameter.");
                return;
            }

            _cards = new Dictionary<Guid, RuntimeCardDefinition>(cards);
            _guids = new List<Guid>();

            foreach(var element in cards)
            {
                _guids.Add(element.Key);

                //GameEvents.OnCardAddedToSet.Trigger(_setName, element.Value);
            }

#if UNITY_EDITOR
            UpdateDebugGuids();
#endif
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardDefinition"></param>
        /// <returns></returns>
        public bool TryGetFirstCard(out RuntimeCardDefinition cardDefinition)
        {
            // Validate the structure.
            if(!IsInitialized() || IsEmpty())
            {
                cardDefinition = default;
                return false;
            }

            // Get the card.
            cardDefinition = _cards[_guids[0]];

            // Validate output.
            if(cardDefinition == null)
            {
                return false;
            }

            return true;
        }


        public bool TryGetFirstCardGuid(out Guid guid)
        {
            if(!IsInitialized() || IsEmpty())
            {
                guid = default;
                return false;
            }

            guid = _guids[0];
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardDefinition"></param>
        /// <returns></returns>
        public bool TryGetLastCard(out RuntimeCardDefinition cardDefinition)
        {
            // Validate the structure.
            if(!IsInitialized() || IsEmpty())
            {
                cardDefinition = default;
                return false;
            }

            // Get the card.
            cardDefinition = _cards[_guids[_guids.Count - 1]];

            // Validate output.
            if(cardDefinition == null)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardGuid"></param>
        /// <param name="cardDefinition"></param>
        /// <returns></returns>
        public bool TryGetCard(Guid cardGuid, out RuntimeCardDefinition cardDefinition)
        {
            // Validate the structure.
            if(!IsInitialized() || IsEmpty())
            {
                cardDefinition = default;
                return false;
            }

            // Get the card and validate output.
            if(!_cards.TryGetValue(cardGuid, out cardDefinition))
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"{_setName}: Card with id [{cardGuid}] is NOT in the set.");
                return false;
            }

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardGuid"></param>
        /// <param name="cardDefinition"></param>
        /// <returns></returns>
        public bool TryAddCard(Guid cardGuid, RuntimeCardDefinition cardDefinition)
        {
            // Validate the list.
            if(!IsInitialized())
            {
                return false;
            }

            // Validate the definition.
            if(cardDefinition == null)
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"{_setName}: Card Definition is null.");
                return false;
            }

            // Check if card is already at the set.
            if(_cards.TryGetValue(cardGuid, out RuntimeCardDefinition dummy))
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"{_setName}: Card with id [{cardGuid}] is already in the set.");
                return false;
            }

            // Add the card.
            _guids.Add(cardGuid);
            _cards.Add(cardGuid, cardDefinition);

            //GameEvents.OnCardAddedToSet.Trigger(_setName, cardDefinition);

#if UNITY_EDITOR
            UpdateDebugGuids();
#endif

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardGuid"></param>
        /// <param name="removedCard"></param>
        /// <returns></returns>
        public bool TryRemoveCard(Guid cardGuid, out RuntimeCardDefinition removedCard)
        {
            // Validate the structure.
            if(!IsInitialized() || IsEmpty())
            {
                removedCard = default;
                return false;
            }

            // Check if card is in the set.
            if(!_cards.TryGetValue(cardGuid, out removedCard))
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"{_setName}: Card with id [{cardGuid}] is NOT in the set.");
                removedCard = default;
                return false;
            }

            // Remove the card.
            if(!_guids.Remove(cardGuid) || !_cards.Remove(cardGuid))
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"{_setName}: Unable to remove the card from the set.");
                removedCard = default;
                return false;
            }

            //GameEvents.OnCardRemovedFromSet.Trigger(_setName, cardGuid);

#if UNITY_EDITOR
            UpdateDebugGuids();
#endif

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Shuffle()
        {
            //_guids.DebugLog();
            _guids.Shuffle();
            //_guids.DebugLog();

            //GameEvents.OnSetCardsOrderChanged.Trigger(_setName, _guids.ToArray());
        }

#if UNITY_EDITOR
        void UpdateDebugGuids()
        {
            _debugGuids = string.Join("\n", _guids);
        }
#endif
    }
}
