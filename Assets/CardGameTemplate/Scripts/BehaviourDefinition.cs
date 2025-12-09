using UnityEngine;

namespace CardGameTemplate
{
    [System.Serializable]
    public struct BehaviourDefinition
    {
        [SerializeField] private BehaviourType _behaviourType;
        [SerializeField] private float _mainValue;
        [SerializeField] private TargetType _targetType;

        public BehaviourType BehaviourType => _behaviourType;
        public float MainValue => _mainValue;
        public TargetType TargetType => _targetType;

        public BehaviourDefinition(BehaviourType behaviourType, float mainValue, TargetType targetType)
        {
            _behaviourType = behaviourType;
            _mainValue = mainValue;
            _targetType = targetType;
        }
    }
}