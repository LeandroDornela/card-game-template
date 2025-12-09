using System.Collections.Generic;

namespace CardGameTemplate
{
    /// <summary>
    /// Represents a generic card behaviour (formerly called “effect”).<br/>
    /// <br/>
    /// A behaviour describes what a card DOES when activated, this may involve 
    /// combat interactions, buffs, debuffs, resource changes, or any other 
    /// gameplay-affecting action.<br/>
    /// <br/>
    /// The term “behaviour” is used instead of “effect” to better reflect actions 
    /// that are not strictly visual or passive, such as combat interactions 
    /// between cards or contextual gameplay responses.<br/>
    /// <br/>
    /// Responsibilities of this class:<br/>
    /// - Define core data for a behaviour (name, primary value, target type).<br/>
    /// - Serve as a base for all card behaviour implementations.<br/>
    /// - Provide an activation flow through <see cref="TryActivateBehaviour"/><br/>
    ///   where derived classes implement their specific logic.<br/>
    /// <br/>
    /// Notes:<br/>
    /// - Additional fields may be added as behaviours become more complex<br/>
    ///   (e.g., secondary values, conditions, cooldowns, costs).<br/>
    /// - Behaviours should remain self-contained and should not manage<br/>
    ///   external game systems beyond applying their intended effects.<br/>
    /// </summary>
    [System.Serializable]
    public abstract class ICardBehaviour
    {
        protected string _behaviourName;
        protected float _mainValue;
        protected TargetType _targetType;

        public string Name => _behaviourName;
        public float MainValue => _mainValue;
        public TargetType TargetType => _targetType;

        public ICardBehaviour(string behaviourName, float mainValue, TargetType targetType)
        {
            _behaviourName = behaviourName;
            _mainValue = mainValue;
            _targetType = targetType;
        }

        public abstract bool TryActivateBehaviour(PlayerState owner, List<IBehaviourTargetWrapper> possibleTargetsToApply);
    }
}
