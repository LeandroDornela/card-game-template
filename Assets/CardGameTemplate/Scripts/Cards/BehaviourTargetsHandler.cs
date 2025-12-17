using System;
using System.Collections.Generic;

namespace CardGameTemplate
{
    public interface IBehaviourTargetsHandler
    {
        List<IBehaviourTarget> GetBehaviourTargets
        (
            PlayerState ownerPlayerState,
            CardBehaviour cardBehaviour,
            Guid targetGuid = default
        );
    }

    public class TargetHandlerOwnerPlayer : IBehaviourTargetsHandler
    {
        public List<IBehaviourTarget> GetBehaviourTargets(PlayerState ownerPlayerState, CardBehaviour cardBehaviour, Guid targetGuid = default)
        {
            if(targetGuid != default && targetGuid != ownerPlayerState.RuntimePlayerGuid)
            {
                Debug.LogError(Debug.Category.Data, $"Card effect target is defined to be the owner player but the target Guid identify a different player.");
                return new List<IBehaviourTarget>();
            }

            return new List<IBehaviourTarget> { ownerPlayerState };
        }
    }

    public class TargetHandlerEnemyPlayers : IBehaviourTargetsHandler
    {
        private readonly CardGameManager _cardGameManager;

        public TargetHandlerEnemyPlayers(CardGameManager cardGameManager)
        {
            _cardGameManager = cardGameManager ?? throw new ArgumentNullException(nameof(cardGameManager));
        }

        public List<IBehaviourTarget> GetBehaviourTargets(PlayerState ownerPlayerState, CardBehaviour cardBehaviour, Guid targetGuid = default)
        {
            List<IBehaviourTarget> targets = new();

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
                    targets.Add(targetPlayerState);
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
                    targets.Add(playerState);
                }
            }

            return targets;
        }
    }
}
