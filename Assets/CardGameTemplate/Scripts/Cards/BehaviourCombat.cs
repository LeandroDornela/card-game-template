using System;
using System.Collections.Generic;

namespace CardGameTemplate
{
    // Yo-gi-oh card monster style with a attack and a defense.
    public class BehaviourCombat : CardBehaviour
    {
        public BehaviourCombat(string effectName, float mainValue, TargetType targetType) : base(effectName, mainValue, targetType)
        {
            throw new NotImplementedException();
        }

        // The combat behaviour is between cards. Defferent from damage heath who damages the player not the player cards.
        public override bool TryActivateBehaviour(PlayerState owner, List<IBehaviourTarget> possibleTargets)
        {
            throw new NotImplementedException();
        }
    }
}
