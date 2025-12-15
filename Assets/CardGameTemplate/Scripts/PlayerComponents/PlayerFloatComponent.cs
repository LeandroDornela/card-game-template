using System;
using UnityEngine;

namespace CardGameTemplate
{
    public class PlayerFloatComponent : IComponent
    {
        private Guid _ownerGuid;

        private float _value = 0;
        private float _maxValue = float.MaxValue;
        private float _minValue = float.MinValue;

        public Guid OwnerGuid => _ownerGuid;
        public float Value => _value;
        public float MaxValue => _maxValue;
        public float MinValue => _minValue;

        /// <summary>
        /// Parameters: (Owner Guid, old value, new value)
        /// </summary>
        public Signal<Guid, float, float> OnValueChanged = new();
        /// <summary>
        /// Parameters: (Owner Guid, old value, new value)
        /// </summary>
        public Signal<Guid, float, float> OnValueIncrease = new();
        /// <summary>
        /// Parameters: (Owner Guid, old value, new value)
        /// </summary>
        public Signal<Guid, float, float> OnValueDecrease = new();
        /// <summary>
        /// Parameters: (Owner Guid, old value, new value)
        /// </summary>
        public Signal<Guid, float, float> OnValueIsMax = new();
        /// <summary>
        /// Parameters: (Owner Guid, old value, new value)
        /// </summary>
        public Signal<Guid, float, float> OnValueIsMin = new();


        public PlayerFloatComponent(Guid ownerGuid, float initialValue, float minValue, float maxValue)
        {
            _ownerGuid = ownerGuid;
            _value = initialValue;
            _minValue = minValue;
            _maxValue = maxValue;
        }


        public bool SetValue(float newValue)
        {
            // Check the input.
            if(newValue == _value)
            {
                Debug.LogWarning(
                    Debug.Category.GameLogic,
                    $"Value unchanged for {_ownerGuid}. Old = {newValue}, attempted set was identical.");
                return true;
            }
            if(newValue > _maxValue)
            {
                Debug.LogError(
                    Debug.Category.GameLogic,
                    $"Attempted to set value above MaxValue for {_ownerGuid}. Attempted = {newValue}, Max = {_maxValue}");
                return false;
            }
            if(newValue < _minValue)
            {
                Debug.LogError(
                    Debug.Category.GameLogic,
                    $"Attempted to set value below MinValue for {_ownerGuid}. Attempted = {newValue}, Min = {_minValue}");
                return false;
            }


            // Change the value.
            float oldValue = _value;
            _value = newValue;


            // Trigger the Actions.
            OnValueChanged.Trigger(_ownerGuid, oldValue, _value);

            if (oldValue < _value)
            {
                OnValueIncrease.Trigger(_ownerGuid, oldValue, _value);
            }
            else
            {
                OnValueDecrease.Trigger(_ownerGuid, oldValue, _value);
            }

            if (_value == _maxValue)
            {
                OnValueIsMax.Trigger(_ownerGuid, oldValue, _value);
            }

            if (_value == _minValue)
            {
                OnValueIsMin.Trigger(_ownerGuid, oldValue, _value);
            }

            return true;
        }

        public bool SafeIncreaseValue(float amount)
        {
            if(amount <= 0)
            {
                Debug.LogError(
                    Debug.Category.GameLogic,
                    $"SafeIncreaseValue amount must be > 0. Owner: {_ownerGuid}, Given: {amount}"
                );
                return false;
            }

            // Change the value.
            float oldValue = _value;
            _value = Mathf.Min(_maxValue, _value + amount);
            
            // Trigger the Actions.
            OnValueChanged.Trigger(_ownerGuid, oldValue, _value);
            OnValueIncrease.Trigger(_ownerGuid, oldValue, _value);
            if(_value == _maxValue)
            {
                OnValueIsMax.Trigger(_ownerGuid, oldValue, _value);
            }

            return true;
        }

        public bool SafeDecreaseValue(float amount)
        {
            if(amount <= 0)
            {
                Debug.LogError(
                    Debug.Category.GameLogic,
                    $"SafeDecreaseValue amount must be > 0. Owner: {_ownerGuid}, Given: {amount}"
                );
                return false;
            }

            // Change the value.
            float oldValue = _value;
            _value = Mathf.Max(_minValue, _value - amount);
            
            // Trigger the Actions.
            OnValueChanged.Trigger(_ownerGuid, oldValue, _value);
            OnValueDecrease.Trigger(_ownerGuid, oldValue, _value);
            if(_value == _minValue)
            {
                OnValueIsMin.Trigger(_ownerGuid, oldValue, _value);
            }

            return true;
        }


        /// <summary>
        /// Will increase or decrease the value depending on the signal.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool SafeChangeValue(float amount)
        {
            if(amount == 0)
            {
                Debug.LogError(
                    Debug.Category.GameLogic,
                    $"SafeChangeValue amount cannot be zero. Owner: {_ownerGuid}"
                );
                return false;
            }

            if(amount > 0)
            {
                return SafeIncreaseValue(amount);
            }
            else
            {
                return SafeDecreaseValue(Mathf.Abs(amount));
            }
        }
    }
}
