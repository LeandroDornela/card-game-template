using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGameTemplate
{
    /// <summary>
    /// Wrapper for storing and manage a set os cards and hide the data structure behind it. 
    /// </summary>
    public class CardSetController
    {
        private CardSetId _setId;
        private Dictionary<Guid, RuntimeCardDefinition> _cards; // Main structure.
        private List<Guid> _guids; // For cards order.
        private bool _initialized = false;

        public CardSetId CardSetId => _setId;
        public string Name => _setId.ToString();

        public IReadOnlyDictionary<Guid, RuntimeCardDefinition> Cards => _cards; // Main structure.
        
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


        public CardSetController(CardSetId setId)
        {
            _setId = setId;
            _cards = new Dictionary<Guid, RuntimeCardDefinition>();
            _guids = new List<Guid>();
            
            _initialized = true;
        }


        public CardSetController(CardSetId setId, Guid[] guids, RuntimeCardDefinition[] cards)
        {
            _setId = setId;
            _cards = new Dictionary<Guid, RuntimeCardDefinition>();
            _guids = new List<Guid>();

            for(int i = 0; i < guids.Length; i++)
            {
                _cards.Add(guids[i], cards[i]); // Cards are referenced, not copied.
                _guids.Add(guids[i]);
            }

            _initialized = true;
        }


        public CardSetController(CardSetId setId, Dictionary<Guid, RuntimeCardDefinition> cards)
        {
            _setId = setId;

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
                Debug.LogError(Debug.Category.Data, $"{_setId}: Card set is not initialized.");
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
                Debug.LogWarning(Debug.Category.Data, $"{_setId}: Card Set is empty.");
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
                Debug.LogError(Debug.Category.Data, $"{_setId}: Invalid input parameter.");
                return;
            }

            _cards = new Dictionary<Guid, RuntimeCardDefinition>(cards);
            _guids = new List<Guid>();

            foreach(var element in cards)
            {
                _guids.Add(element.Key);
            }
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
                Debug.LogError(Debug.Category.Data, $"{_setId}: Card with id [{cardGuid}] is NOT in the set.");
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
                Debug.LogError(Debug.Category.Data, $"{_setId}: Card Definition is null.");
                return false;
            }

            // Check if card is already at the set.
            if(_cards.TryGetValue(cardGuid, out _))
            {
                Debug.LogError(Debug.Category.Data, $"{_setId}: Card with id [{cardGuid}] is already in the set.");
                return false;
            }

            // Add the card.
            _guids.Add(cardGuid);
            _cards.Add(cardGuid, cardDefinition);

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
                Debug.LogError(Debug.Category.Data, $"{_setId}: Card with id [{cardGuid}] is NOT in the set.");
                removedCard = default;
                return false;
            }

            // Remove the card.
            if(!_guids.Remove(cardGuid) || !_cards.Remove(cardGuid))
            {
                Debug.LogError(Debug.Category.Data, $"{_setId}: Unable to remove the card from the set.");
                removedCard = default;
                return false;
            }

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
        }
    }
}
