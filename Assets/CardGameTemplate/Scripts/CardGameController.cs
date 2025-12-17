using System;
using System.Collections.Generic;

namespace CardGameTemplate
{
    /// <summary>
    /// - Provide methods to control the card game logic in detail. Things like: move cards,
    /// activate cards, shuffle decks, check victory conditions, etc.<br/>
    /// - Modify data stored in the Card Game Manager.<br/>
    /// </summary>
    public class CardGameController
    {
        private CardGameManager _cardGameManager;
        private bool _decksInitialized = false;


        public CardGameController(CardGameManager cardGameManager)
        {
            _cardGameManager = cardGameManager;
        }
        

#region ==================== Private Methods ====================


        List<IBehaviourTargetWrapper> GetBehaviourTargets(PlayerState ownerPlayerState, CardBehaviour cardBehaviour, Guid targetGuid = default)
        {
            List<IBehaviourTargetWrapper> targets = new List<IBehaviourTargetWrapper>();

            switch(cardBehaviour.TargetType)
            {
                case TargetType.OwnerPlayer:
                    if(targetGuid != default && targetGuid != ownerPlayerState.RuntimePlayerGuid)
                    {
                        Debug.LogError(Debug.Category.Data, $"Card effect is the card owner but given target Guid don't match owner Guid.");
                        return targets;
                    }
                    else
                    {
                        targets.Add(new PlayerBehaviourTarget(ownerPlayerState));
                        return targets;
                    }
                case TargetType.EnemyPlayers:
                    // If Guid defined apply to one, else apply to all enemies.
                    if(targetGuid != default)
                    {
                        if(targetGuid == ownerPlayerState.RuntimePlayerGuid)
                        {
                            Debug.LogError(Debug.Category.Data, $"Card effect target is defined to be a enemy but the target Guid identify the card owner player.");
                            return targets;
                        }

                        // If have a target guid, find and add the player.
                        if(_cardGameManager.TryGetPlayerState(targetGuid, out PlayerState targetPlayerState))
                        {
                            targets.Add(new PlayerBehaviourTarget(targetPlayerState));
                            return targets;
                        }
                        else
                        {
                            Debug.LogError(Debug.Category.Data, $"Can't find player with Guid: {targetGuid}.");
                            return targets;
                        }
                    }
                    else // If don't have a definite target guid. Add all other players.
                    {
                        _cardGameManager.TryGetPlayerStatesNotGuid(ownerPlayerState.RuntimePlayerGuid, out var enemiesPlayerStates);

                        foreach(PlayerState playerState in enemiesPlayerStates)
                        {
                            targets.Add(new PlayerBehaviourTarget(playerState));
                        }

                        return targets;
                    }
                default:
                    throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Try to add cards to all free slots in hand. The return result not necessarily indicate an error.
        /// </summary>
        /// <param name="playerState"></param>
        /// <returns>True -> hand is full. Else -> still having empty slots.</returns>
        bool TryFillHand(PlayerState playerState)
        {
            // Get the number of card to draw to avoid using a while loop.
            if(!playerState.TryGetCardSetController(CardSetId.Hand, out CardSetController handCards)) return false;
            int cardsToDraw = _cardGameManager.HandSlots - handCards.Count;

            for(int i = 0; i < cardsToDraw; i++)
            {
                if(!TryDrawACard(playerState))
                {
                    // Break the loop when can't draw a card. The reason can
                    // be by a error or just by a empty deck.
                    // TODO: Better to know here if it was an error.
                    break;
                }
            }

            return handCards.Count == _cardGameManager.HandSlots;
        }


        bool TryDrawACard(PlayerState playerState)
        {
            // Try get Hand and deck cards sets.
            if(!playerState.TryGetCardSetController(CardSetId.Hand, out CardSetController handCards)) return false;
            if(!playerState.TryGetCardSetController(CardSetId.Deck, out CardSetController deckCards)) return false;

            // Avoid trying to get a card from empty deck, what will throw errors.
            if(deckCards.Count == 0)
            {
                // Not a error since deck com be empty. Just can't draw.
                Debug.Log(Debug.Category.GameLogic, $"{playerState.PlayerName} deck is empty.");
                return false;
            }

            // Also check if the hand is full.
            if(handCards.Count >= _cardGameManager.HandSlots)
            {
                Debug.Log(Debug.Category.GameLogic, $"{playerState.PlayerName} can't add card from deck to hand. Hand is full.");
                return false;
            }

            if(!deckCards.TryGetFirstCard(out RuntimeCardDefinition cardGuid))
            {
                Debug.LogError(Debug.Category.GameLogic, $"{playerState.PlayerName} can't get first card Guid from deck. Failed to draw card.");
                return false;
            }

            return CardFromDeckToHand(cardGuid, playerState);
        }


        bool CardFromDeckToHand(RuntimeCardDefinition cardDefinition, PlayerState playerState)
        {
            // Try get Hand and Deck cards sets.
            if(!playerState.TryGetCardSetController(CardSetId.Hand, out CardSetController handCards)) return false;
            if(!playerState.TryGetCardSetController(CardSetId.Deck, out CardSetController deckCards)) return false;

            // Try move the card
            if(!MoveCardFromTo(cardDefinition, deckCards, handCards)) return false;

            // Trigger the signals
            GameEvents.OnCardRemoved_Deck.Trigger(playerState.RuntimePlayerGuid, cardDefinition);
            GameEvents.OnCardAdded_Hand.Trigger(playerState.RuntimePlayerGuid, cardDefinition);
            // Better for just a animation of the card UI element going from deck to hand.
            GameEvents.OnCardMoved_DeckToHand.Trigger(playerState.RuntimePlayerGuid, cardDefinition);

            return true;
        }


        bool CardFromHandToDiscarded(RuntimeCardDefinition cardDefinition, PlayerState playerState)
        {
            // Try get Hand and Discarded cards sets.
            if(!playerState.TryGetCardSetController(CardSetId.Hand, out CardSetController handCards)) return false;
            if(!playerState.TryGetCardSetController(CardSetId.Discarded, out CardSetController discardedCards)) return false;

            // Try move the card
            if(!MoveCardFromTo(cardDefinition, handCards, discardedCards)) return false;

            // Trigger the signals
            GameEvents.OnCardRemoved_Hand.Trigger(playerState.RuntimePlayerGuid, cardDefinition);
            GameEvents.OnCardAdded_Discarded.Trigger(playerState.RuntimePlayerGuid, cardDefinition);
            GameEvents.OnCardMoved_HandToDiscarded.Trigger(playerState.RuntimePlayerGuid, cardDefinition);

            return true;
        }


        bool MoveCardFromTo(RuntimeCardDefinition cardDefinition, CardSetId fromSet, CardSetId toSet)
        {
            throw new NotImplementedException();
        }


        bool MoveCardFromTo(RuntimeCardDefinition cardDefinition, CardSetController fromSet, CardSetController toSet)
        {
            if(!fromSet.TryRemoveCard(cardDefinition.RuntimeCardGuid, out RuntimeCardDefinition removedCard))
            {
                Debug.LogError(Debug.Category.GameLogic, $"Failed to remove card from {fromSet.Name}.");
                return false;
            }

            if(!toSet.TryAddCard(removedCard.RuntimeCardGuid, removedCard))
            {
                Debug.LogError(Debug.Category.GameLogic, $"Failed to add card to {toSet.Name}.");
                return false;
            }

            Debug.Log(Debug.Category.GameLogic, $"Moved card [{cardDefinition}] from {fromSet.Name} to {toSet.Name}");
            return true;
        }
#endregion


#region ==================== Public Methods ====================
        public void FillPlayersDecks()
        {
            // For each player state, overwrite the deck with the cards from the player profile
            foreach(var playerStatePair in _cardGameManager.PlayerStates)
            {
                if(!playerStatePair.Value.TryGetCardSetController(CardSetId.Deck, out CardSetController deckCards)) return;

                deckCards.Overwrite(_cardGameManager.PlayerProfiles[playerStatePair.Key].DeckCardDefinitions);
            }

            _decksInitialized = true;
        }

        
        public void ShufflePlayersDecks()
        {
            if(!_decksInitialized)
            {
                Debug.LogError(Debug.Category.Data, $"Decks are not initialized. Use {nameof(FillPlayersDecks)} to add the cards from player profile.");
                return;
            }

            foreach(var playerStatePair in _cardGameManager.PlayerStates)
            {
                if(!playerStatePair.Value.TryGetCardSetController(CardSetId.Deck, out CardSetController deckCards)) return;

                deckCards.Shuffle();
            }
        }


        public void TryFillPlayersHands()
        {
            if(!_decksInitialized)
            {
                Debug.LogError(Debug.Category.Data, $"Decks are not initialized. Use {nameof(FillPlayersDecks)} to add the cards from player profile.");
                return;
            }

            foreach(var playerStatePair in _cardGameManager.PlayerStates)
            {
                TryFillHand(playerStatePair.Value);
            }
        }

        
        public bool TryDrawACard(Guid playerStateGuid)
        {
            if(_cardGameManager.TryGetPlayerState(playerStateGuid, out PlayerState playerState))
            {
                return TryDrawACard(playerState);
            }
            else
            {
                return false;
            }
        }


        public bool TryUseCard(Guid cardOwnerGuid, Guid runtimeCardGuid, Guid targetGuid = default)
        {
            // Get owner player state.
            if(!_cardGameManager.TryGetPlayerState(cardOwnerGuid, out PlayerState ownerPlayerState))
            {
                Debug.LogError(Debug.Category.Data, "Can't find Player State with the given Guid");
                return false;
            }

            // Get the hand cards
            if(!ownerPlayerState.TryGetCardSetController(CardSetId.Hand, out CardSetController handCards)) return false;
            
            // Get the card definition from the guid.
            if(!handCards.TryGetCard(runtimeCardGuid, out RuntimeCardDefinition cardToUseDefinition))
            {
                Debug.LogError(Debug.Category.Data, "Can't find Card Definition at player hand with the given Guid");
                return false;
            }

            return TryUseCard(ownerPlayerState, cardToUseDefinition);
        }


        public bool TryUseCard(PlayerState ownerPlayerState, RuntimeCardDefinition cardToUseDefinition, Guid targetGuid = default)
        {
            // Try apply each effect to each target.
            foreach(CardBehaviour behaviour in cardToUseDefinition.Effects)
            {
                List<IBehaviourTargetWrapper> targets = GetBehaviourTargets(ownerPlayerState, behaviour, targetGuid);

                if(!behaviour.TryActivateBehaviour(ownerPlayerState, targets))
                {
                    Debug.LogError(Debug.Category.GameLogic, $"Can't apply effect to targets.");
                    return false;
                }
            }

            return CardFromHandToDiscarded(cardToUseDefinition, ownerPlayerState);
        }
#endregion
    }
}
