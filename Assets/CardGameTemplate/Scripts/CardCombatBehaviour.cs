using System;
using System.Collections.Generic;

using UnityEngine;

namespace CardGameTemplate
{
    // Yo-gi-oh card monster style with a attack and a defense.
    public class CardCombatBehaviour : ICardBehaviour
    {
        public CardCombatBehaviour(string effectName, float mainValue, TargetType targetType) : base(effectName, mainValue, targetType)
        {
            throw new NotImplementedException();
        }

        // The combat behaviour is between cards. Defferent from damage heath who damages the player not the player cards.
        public override bool TryActivateBehaviour(PlayerState owner, List<IBehaviourTargetWrapper> objectToApply)
        {
            throw new NotImplementedException();
        }
    }
}
