using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CardGameTemplate
{
    /// <summary>
    /// - Instantiate and initialize base game elements(not game logic or game specific related). And store them.<br/>
    /// - Don't change the data of instantiated objects(player states, match state) directly. This is responsibility of
    ///   the Controller. Again, only store that data.<br/>
    /// - Holds th game state machine. The state machine controls the game flow and calls what will happen(not how).<br/>
    /// 
    /// Main Scene bootstrap. Instantiate and store main game elements like:<br/>
    /// - MatchState: Stores runtime match data like match timer or current turn for turn based card games.<br/>
    /// - Player profiles: players data with elements like name and starting cards in deck.<br/>
    /// - Players controllers: the controllers who live in the scene and are responsible for getting input from human
    /// players, or make decisions for AI players.<br/>
    /// - PlayersStates: who stores the current states of the players, with elements like health and players cards sets.<br/>
    /// - CardGameController: who change the players and match state.<br/>
    /// </summary>
    public class CardGameManager : Singleton<CardGameManager> // World
    {
        [SerializeField] private CardGameConfig _cardGameConfig;
        
        private GameStateMachine _stateMachine;
        private CardGameController _cardGameController;
        private MatchState _matchState; // Runtime data(state) of the match. NOT logical state of State Machine.

        // Players
        private readonly Dictionary<Guid, RuntimePlayerProfile> _playerProfiles = new Dictionary<Guid, RuntimePlayerProfile>();
        private readonly Dictionary<Guid, Controller> _controllers = new Dictionary<Guid, Controller>();
        private readonly Dictionary<Guid, PlayerState> _states = new Dictionary<Guid, PlayerState>();

        private int _handSlots = 5;


#region ==================== Public Parameters and Access Methods ====================

        public CardGameController CardGameController => _cardGameController;
        public MatchState MatchState => _matchState;
        public IReadOnlyDictionary<Guid, RuntimePlayerProfile> PlayerProfiles => _playerProfiles;
        public IReadOnlyDictionary<Guid, Controller> PlayerControllers => _controllers;
        public IReadOnlyDictionary<Guid, PlayerState> PlayerStates => _states;
        public int HandSlots => _handSlots;

        public bool TryGetPlayerProfile(Guid guid, out RuntimePlayerProfile profile)
        {
            return _playerProfiles.TryGetValue(guid, out profile);
        }

        public bool TryGetPlayerController(Guid guid, out Controller controller)
        {
            return _controllers.TryGetValue(guid, out controller);
        }

        public bool TryGetPlayerState(Guid guid, out PlayerState playerState)
        {
            return _states.TryGetValue(guid, out playerState);
        }

        public bool IsInGame()
        {
            return _stateMachine.CurrentState == InGameStateId.InGame;
        }

        public InGameStateId GetCurrentGameState()
        {
            return _stateMachine.CurrentState;
        }

        /// <summary>
        /// Try get all Player States that Runtime Guid don't match with the give Guid.
        /// </summary>
        public bool TryGetPlayerStatesNotGuid(Guid guid, out PlayerState[] playerStates)
        {
            playerStates = _states.Where(kvp => kvp.Key != guid).Select(kvp => kvp.Value).ToArray();
            
            // Validate the result.
            if(playerStates != null && playerStates.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

#endregion


#region ==================== MonoBehaviour Callbacks ====================

        void Awake()
        {
            if(!_cardGameConfig)
            {
                Debug.LogError(Debug.Category.Data, "Card Game Config is undefined.");
                return;
            }

            _cardGameController = new CardGameController(this);
            _matchState = new MatchState();

            // Create the state machine, what will trigger the Initializing state by default.
            _stateMachine = new GameStateMachine(new Dictionary<InGameStateId, IGameState>
            {
                { InGameStateId.Initializing, new StateInitializing(this)},
                { InGameStateId.InGame,new StateInGame(this)},
                { InGameStateId.Paused,new StatePaused(this)},
                { InGameStateId.GameOver,new StateGameOver(this)}
            });

            _stateMachine.StartMachine(InGameStateId.Initializing);

            _handSlots = _cardGameConfig.MaxCardsInHand;
        }


        void Start()
        {
            
        }


        void Update()
        {
            _stateMachine.Update();
        }

#endregion


#region ==================== Private Methods ====================

        /// <summary>
        /// By default the local player is the first on the profiles.
        /// </summary>
        void SetupPlayers(RuntimePlayerProfile[] playersInGame)
        {
            if(playersInGame == null || playersInGame.Length == 0)
            {
                Debug.LogError(Debug.Category.Data, "Invalid Players Profiles.");
                return;
            }

            foreach(RuntimePlayerProfile player in playersInGame)
            {
                AddPlayer(player);
            }
        }


        /// <summary>
        /// Create and initialize a new Player State and Controller using the Player Profile.
        /// </summary>
        /// <param name="playerProfile"></param>
        void AddPlayer(RuntimePlayerProfile playerProfile)
        {
            if(playerProfile == default)
            {
                Debug.LogError(Debug.Category.Data, "Invalid Player Profile.");
                return;
            }

            if(_playerProfiles.TryGetValue(playerProfile.RuntimePlayerGuid, out _))
            {
                Debug.LogError(Debug.Category.Data, $"Player already created. Guid: [{playerProfile.RuntimePlayerGuid}]");
                return;
            }

            // Select the correct controller based on player type.
            if(!_cardGameConfig.ControllerPrefabsDefinitions.TryGetPrefab(playerProfile.PlayerType, out GameObject controllerPrefab))
            {
                Debug.LogError(Debug.Category.Data, $"Invalid or unsupported Player Type: {playerProfile.PlayerType}");
                return;
            }

            // Instantiate the new controller and try to get the Controller component, if can't, stops execution.
            GameObject newControllerGo = Instantiate(controllerPrefab, transform);
            if(!newControllerGo.TryGetComponent(out Controller newController))
            {
                Debug.LogError(Debug.Category.Data, $"Unable to get Controller for player {playerProfile.PlayerName}");
                return;
            }


            //
            // If all data is valid, create and initialize the rest of player related objects.
            //

            // Add the PROFILE.
            _playerProfiles.Add(playerProfile.RuntimePlayerGuid, playerProfile);

            // Create and add the new PLAYER STATE to the states dictionary.
            PlayerState newPlayerState = new PlayerState(playerProfile.RuntimePlayerGuid, playerProfile.PlayerName, playerProfile.DefaultComponentsStats);
            _states.Add(playerProfile.RuntimePlayerGuid, newPlayerState);
    
            // After creation, initialize and add the new CONTROLLER to the dictionary.
            newController.Initialize(playerProfile.RuntimePlayerGuid);
            _controllers.Add(playerProfile.RuntimePlayerGuid, newController);

            // Trigger the new player signal.
            GameEvents.OnNewPlayerAdded.Trigger(newPlayerState);
        }
#endregion


#region ==================== Public Methods ====================

        public async void SetupMatch(Action onFinished)
        {
            // Get match data.
            var matchData = _cardGameConfig.CardGameDataManager.GetCardGameMatchData();
            
#if SIMULATE_NETWORK_DELAY
            // Simulate a network delay
            await Task.Delay(5000);
#endif

            // Setup the match players.
            SetupPlayers(matchData.RuntimePlayerProfiles);

            onFinished.Invoke();
        }

#endregion
    }
}
