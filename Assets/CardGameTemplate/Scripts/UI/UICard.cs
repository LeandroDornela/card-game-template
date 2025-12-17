using System;
using TMPro;
using UnityEngine;

namespace CardGameTemplate
{
    /// <summary>
    /// Just for testing purposes.
    /// </summary>
    [Obsolete]
    public class UICard : MonoBehaviour
    {
        public TMP_Text _cardName;
        public TMP_Text _cardDescription;
        public TMP_Text _cardEffects;

        public TMP_Text _cardGuidText;
        public TMP_Text _ownerGuidText;

        private Guid _cardGuid;
        private Guid _ownerGuid;


        public void Initialize(Guid ownerGuid, RuntimeCardDefinition cardDefinition)
        {
            _cardName.SetText(cardDefinition.CardName);
            _cardDescription.SetText(cardDefinition.Description);
            
            string effects = "";
            foreach(var effect in cardDefinition.Effects)
            {
                effects += $"{effect.Name}\n";
            }
            _cardEffects.SetText(effects);

            _cardGuidText.SetText(cardDefinition.RuntimeCardGuid.ToString());
            _ownerGuidText.SetText(ownerGuid.ToString());

            _cardGuid = cardDefinition.RuntimeCardGuid;
            _ownerGuid = ownerGuid;
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        [Obsolete]
        public void TEST_Use()
        {
            GameEvents.OnGameActionRequest.Trigger(new ActionUseCard(_ownerGuid, _cardGuid));
        }

        [Obsolete]
        public void TEST_Deck()
        {
            
        }

        [Obsolete]
        public void TEST_Discard()
        {
            
        }
    }
}
