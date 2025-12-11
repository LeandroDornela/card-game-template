using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGameTemplate
{
    public class CardGameViewModel : MonoBehaviour
    {
        [SerializeField] private UICard _cardPrefab;

        public TMP_Text _timer;
        public TMP_Text _playerHealth;
        public TMP_Text _enemyName;
        public TMP_Text _enemyHealth;
        public HorizontalLayoutGroup _cardsGroup;


        private Controller _ownerController;
        private Dictionary<Guid, UICard> _ownerCards;

        // 1º
        void Awake()
        {
            _ownerCards = new Dictionary<Guid, UICard>();
        }

        // 2º
        void OnEnable()
        {
            GameEvents.OnNewPlayerAdded.AddListener(OnNewPlayer);
            GameEvents.OnCardAdded_Hand.AddListener(OnCardAddedToHand);
            GameEvents.OnCardRemoved_Hand.AddListener(OnCardRemovedFromHand);
            GameEvents.OnPlayerHealthChanged.AddListener(OnPlayerHPChanged);
            GameEvents.OnMatchTimerUpdate.AddListener(OnTimerUpdate);
        }

        // 3º
        public void Initialize(Controller ownerController)
        {
            _ownerController = ownerController;
        }

        // 4º
        void Start()
        {
            
        }

        void OnDisable()
        {
            GameEvents.OnNewPlayerAdded.RemoveListener(OnNewPlayer);
            GameEvents.OnCardAdded_Hand.RemoveListener(OnCardAddedToHand);
            GameEvents.OnCardRemoved_Hand.RemoveListener(OnCardRemovedFromHand);
            GameEvents.OnPlayerHealthChanged.RemoveListener(OnPlayerHPChanged);
            GameEvents.OnMatchTimerUpdate.RemoveListener(OnTimerUpdate);
        }


        void OnNewPlayer(PlayerState playerState)
        {
            if(_ownerController.RuntimePlayerGuid == default)
            {
                CardGameTemplate.Debug.LogError(Debug.Category.UI, "Owner Guid is undefined.");
                return;
            }

            if(playerState.RuntimePlayerGuid == _ownerController.RuntimePlayerGuid)
            {
                _playerHealth.SetText(playerState.Health.Value.ToString());
            }
            else
            {
                _enemyHealth.SetText(playerState.Health.Value.ToString());
                _enemyName.SetText(playerState.PlayerName.ToString());
            }
        }

        void OnCardAddedToHand(Guid ownerGuid, RuntimeCardDefinition runtimeCardDefinition)
        {
            if(_ownerController.RuntimePlayerGuid != ownerGuid)
            {
                // The card is now from the owner of that UI.
                return;
            }

            UICard newUICard = Instantiate(_cardPrefab.gameObject).GetComponent<UICard>();
            newUICard.Initialize(ownerGuid, runtimeCardDefinition);
            
            _ownerCards.Add(runtimeCardDefinition.RuntimeCardGuid, newUICard);

            newUICard.transform.SetParent(_cardsGroup.transform);
        }

        void OnCardRemovedFromHand(Guid ownerGuid, RuntimeCardDefinition runtimeCardDefinition)
        {
            if(_ownerController.RuntimePlayerGuid != ownerGuid)
            {
                // The card is now from the owner of that UI.
                return;
            }

            Destroy(_ownerCards[runtimeCardDefinition.RuntimeCardGuid].gameObject);
            _ownerCards.Remove(runtimeCardDefinition.RuntimeCardGuid);
        }

        void OnPlayerHPChanged(Guid ownerGuid, float amount)
        {
            if(ownerGuid == _ownerController.RuntimePlayerGuid)
            {
                _playerHealth.SetText(amount.ToString());
            }
            else
            {
                _enemyHealth.SetText(amount.ToString());
            }
        }


        void OnTimerUpdate(float value)
        {
            _timer.SetText(value.ToString());
        }


        public void TEMP_DrawCard()
        {
            CardGameManager.Instance.CardGameController.TryDrawACard(_ownerController.RuntimePlayerGuid);
        }

        public void TEMP_Pause()
        {
            GameEvents.OnKeyPressed_Pause.Trigger();
        }
    }
}
