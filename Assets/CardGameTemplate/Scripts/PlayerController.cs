using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CardGameTemplate
{
    public class PlayerController : Controller
    {
        [SerializeField] private CardGameViewModel cardGameViewModelPrefab; // In Game UI related to de the cards and gameplay.

        private CardGameViewModel _cardGameViewModelInstance;

        
        
        // 1º
        void Awake()
        {
            
        }

        // 2º
        public override void Initialize(Guid runtimePlayerGuid)
        {
            base.Initialize(runtimePlayerGuid);

            _cardGameViewModelInstance = Instantiate(cardGameViewModelPrefab, transform).GetComponent<CardGameViewModel>();
            _cardGameViewModelInstance.Initialize(this);
        }

        // 3º
        void Start()
        {
            
        }


        // Update is called once per frame
        void Update()
        {
            
        }

        public void Pause(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                GameEvents.OnKeyPressed_Pause.Trigger();
            }
        }
    }
}
