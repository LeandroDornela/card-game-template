using System.Collections.Generic;

namespace CardGameTemplate
{
    public class BehaviourDamageHealth : ICardBehaviour
    {
        public BehaviourDamageHealth(string behaviourName, float damage, TargetType targetType) : base(behaviourName, damage, targetType){}

        public override bool TryActivateBehaviour(PlayerState owner, List<IBehaviourTargetWrapper> possibleTargetsToApply)
        {
            CardGameTemplate.Debug.Log(Debug.Category.GameLogic, $"Trying to apply effect {GetType()}.");

            if(possibleTargetsToApply == null)
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"{nameof(possibleTargetsToApply)} is null.");
                return false;
            }

            bool success = false;

            foreach(IBehaviourTargetWrapper targetWrapper in possibleTargetsToApply)
            {
                PlayerState playerState = targetWrapper.GetSubTargetsHolder<PlayerState>();

                if(playerState == null)
                {
                    // It's expected to only receive targets that will be used by the effect.
                    // Give a warning and continue to next iteration.
                    CardGameTemplate.Debug.LogWarning(Debug.Category.GameLogic, $"Received a target unusable by {GetType()}.");
                    continue;
                }

                if(playerState.ComponentHealth == null)
                {
                    CardGameTemplate.Debug.LogError(Debug.Category.GameLogic, "Health Component is invalid.");
                    return false;
                }

                // At least one of the targets must be valid, a Player State.
                // And receive the damage.
                if(playerState.ComponentHealth.TakeDamage(_mainValue))
                {
                    success = true;
                }
            }

            // Success if at least one target is valid.
            return success;
        }
    }
}
