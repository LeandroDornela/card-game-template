using System;
using UnityEngine;

namespace CardGameTemplate
{
    public class ComponentHealth : IComponent
    {
        private Guid _ownerGuid;

        [SerializeField] private float _health = 100;
        [SerializeField] private float _maxValue = 100;

        public Guid OwnerGuid => _ownerGuid;
        public float Health => _health;


        public void Initialize(Guid ownerGuid, float maxValue, float initialValue)
        {
            _ownerGuid = ownerGuid;
            _health = initialValue;
            _maxValue = maxValue;
        }


        public bool TakeDamage(float amount)
        {
            if(amount <= 0)
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"Invalid damage amount: {amount}");
                return false;
            }

            if(_health == 0)
            {
                CardGameTemplate.Debug.LogWarning(Debug.Category.GameLogic, $"No health, can't take damage.");
                return false;
            }

            // Apply the damage.
            float healthBefore = _health;
            _health = Mathf.Max(0, _health - amount);

            // Trigger Signals
            GameEvents.OnPlayerHealthChanged.Trigger(_ownerGuid, _health);
            //OnHealthDecrease.Trigger(_ownerGuid, healthBefore - _health);
            
            if(_health == 0)
            {
                GameEvents.OnPlayerHealthIsZero.Trigger(_ownerGuid);
            }

            CardGameTemplate.Debug.Log(Debug.Category.GameLogic, $"Damage received: {amount}");

            return true;
        }


        public bool Cure(float amount)
        {
            if(amount <= 0)
            {
                CardGameTemplate.Debug.LogError(Debug.Category.Data, $"Invalid cure amount: {amount}");
                return false;
            }

            if(_health == _maxValue)
            {
                // Max health, can't cure.
                return false;
            }

            // Apply Cure
            float healthBefore = _health;
            _health = Mathf.Min(100, _health + amount);

            // Trigger Signals
            GameEvents.OnPlayerHealthChanged.Trigger(_ownerGuid, _health);
            //OnHealthIncrease.Trigger(_ownerGuid, _health - healthBefore);

            if(_health == _maxValue)
            {
                //OnMaxHealth.Trigger(_ownerGuid);
            }

            return true;
        }
    }
}
