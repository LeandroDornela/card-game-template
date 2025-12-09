using System.Collections.Generic;

namespace CardGameTemplate
{
    // Wrapper for storing and manage a set os cards and hide the data structure behind it. 
    [System.Serializable]
    public class TCardSetController<IdType, CardType>
    {
        private string _setName;
        private Dictionary<IdType, CardType> _cards; // Main structure.
        private List<IdType> _guids; // For cards order.
        
        private bool _initialized = false;


        public string Name => _setName;
        public int Count => _guids.Count;


        public TCardSetController(string setName)
        {
            _setName = setName;
            _cards = new Dictionary<IdType, CardType>();
            _guids = new List<IdType>();
            
            _initialized = true;
        }

        public TCardSetController(string setName, IdType[] guids, CardType[] cards)
        {
            _setName = setName;
            _cards = new Dictionary<IdType, CardType>();
            _guids = new List<IdType>();

            for(int i = 0; i < guids.Length; i++)
            {
                _cards.Add(guids[i], cards[i]); // Cards are referenced, not copied.
                _guids.Add(guids[i]);
            }

            _initialized = true;
        }

        public TCardSetController(string setName, Dictionary<IdType, CardType> cards)
        {
            _setName = setName;

            Overwrite(cards);

            _initialized = true;
        }


        bool IsInitialized()
        {
            if(!_initialized || _cards == null || _guids == null)
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"{_setName}: Card set is not initialized.");
                return false;
            }

            return true;
        }


        bool IsEmpty()
        {
            if(_cards?.Count == 0)
            {
                CardGameTemplate.Debug.LogWarning(Debug.Category.Data, $"{_setName}: Card Set is empty.");
                return true;
            }

            return false;
        }


        public void Overwrite(Dictionary<IdType, CardType> cards)
        {
            if(cards == null)
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"{_setName}: Invalid input parameter.");
                return;
            }

            _cards = cards; // Cards are referenced, not copied.
            _guids = new List<IdType>();

            foreach(var element in cards)
            {
                _guids.Add(element.Key);
            }
        }


        public bool TryGetFirstCard(out CardType cardDefinition)
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


        public bool TryGetLastCard(out CardType cardDefinition)
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


        public bool TryGetCard(IdType cardGuid, out CardType cardDefinition)
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

        public bool TryAddCard(IdType cardGuid, CardType cardDefinition)
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
            if(_cards.TryGetValue(cardGuid, out CardType dummy))
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"{_setName}: Card with id [{cardGuid}] is already in the set.");
                return false;
            }

            // Add the card.
            _guids.Add(cardGuid);
            _cards.Add(cardGuid, cardDefinition);

            return true;
        }


        public bool TryRemoveCard(IdType cardGuid, out CardType removedCard)
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

            return true;
        }


        public void Shuffle()
        {
            //_guids.DebugLog();
            _guids.Shuffle();
            //_guids.DebugLog();
        }
    }
}
