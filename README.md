# Card Game Template

---
# Preview Doc
---
## Table of Contents
1. [Executive Summary](#executive-summary)
2. [Architectural Overview](#architectural-overview)
3. [Core Systems](#core-systems)
4. [Design Patterns & Principles](#design-patterns--principles)
5. [Data Flow Architecture](#data-flow-architecture)
6. [Component Deep Dive](#component-deep-dive)
7. [Extensibility & Scalability](#extensibility--scalability)
8. [Technical Debt & Considerations](#technical-debt--considerations)
9. [Best Practices & Conventions](#best-practices--conventions)

---

## Executive Summary

### Project Purpose
A flexible, extensible card game framework built in Unity/C# designed to support multiple card game genres (CCG, TCG, deck-builders) with emphasis on clean architecture, maintainability, and network-readiness.

### Key Characteristics
- **Architecture**: Event-driven, component-based, state-machine controlled
- **Coupling**: Loose coupling via interfaces and observer pattern
- **Extensibility**: Plugin architecture for cards, effects, and controllers
- **Scalability**: Designed for both local and networked multiplayer
- **Code Quality**: Strong typing, null safety, comprehensive error handling

### Technology Stack
- **Language**: C# 
- **Engine**: Unity (MonoBehaviour-based)
- **UI**: Unity UI with TextMeshPro
- **Dependencies**: SerializedCollections (third-party)

---

## Architectural Overview

### High-Level Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                     PRESENTATION LAYER                       │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │CardGameView  │  │   UICard     │  │PauseMenu(etc)│      │
│  │   Model      │  │              │  │              │      │
│  └──────┬───────┘  └──────────────┘  └──────────────┘      │
└─────────┼───────────────────────────────────────────────────┘
          │ subscribes to events
          ▼
┌─────────────────────────────────────────────────────────────┐
│                      EVENT BUS LAYER                         │
│  ┌────────────────────────────────────────────────────┐     │
│  │              GameEvents (Static)                   │     │
│  │  • OnCardAdded_Hand    • OnPlayerHealthChanged     │     │
│  │  • OnEnterState_InGame • OnGameActionRequest       │     │
│  └────────────────────────────────────────────────────┘     │
└─────────────────────────────────────────────────────────────┘
          ▲                           │
          │ publishes               │ subscribes
          │                           ▼
┌─────────┴───────────────────────────────────────────────────┐
│                   GAME LOGIC LAYER                           │
│  ┌──────────────────────────────────────────────────────┐   │
│  │           CardGameManager (Singleton)                 │   │
│  │  ┌────────────────┐  ┌────────────────────────────┐  │   │
│  │  │ State Machine  │  │   ActionHandlersManager    │  │   │
│  │  │  • Initialize  │  │   • DrawCard Handler       │  │   │
│  │  │  • InGame      │  │   • UseCard Handler        │  │   │
│  │  │  • Paused      │  │                            │  │   │
│  │  │  • GameOver    │  │                            │  │   │
│  │  └────────────────┘  └────────────────────────────┘  │   │
│  │                                                        │   │
│  │  ┌────────────────────────────────────────────────┐  │   │
│  │  │       CardGameController (Facade)              │  │   │
│  │  │  • TryFillPlayersHands()                       │  │   │
│  │  │  • TryUseCard()                                │  │   │
│  │  │  • MoveCardFromTo()                            │  │   │
│  │  └────────────────────────────────────────────────┘  │   │
│  └──────────────────────────────────────────────────────┘   │
│                                                               │
│  ┌──────────────────────────────────────────────────────┐   │
│  │              Player Controllers                       │   │
│  │  ┌──────────────┐        ┌──────────────┐           │   │
│  │  │PlayerControl │        │AIController  │           │   │
│  │  │(Human Input) │        │(AI Logic)    │           │   │
│  │  └──────────────┘        └──────────────┘           │   │
│  └──────────────────────────────────────────────────────┘   │
└───────────────────────────────────────────────────────────────┘
          │
          ▼
┌─────────────────────────────────────────────────────────────┐
│                      DATA/STATE LAYER                        │
│  ┌────────────────┐  ┌─────────────────────────────────┐   │
│  │  PlayerState   │  │      MatchState                 │   │
│  │  • Health      │  │      • Timer                    │   │
│  │  • Mana        │  │                                 │   │
│  │  • CardSets    │  │                                 │   │
│  │    - Deck      │  │                                 │   │
│  │    - Hand      │  │                                 │   │
│  │    - InGame    │  │                                 │   │
│  │    - Discarded │  │                                 │   │
│  └────────────────┘  └─────────────────────────────────┘   │
│                                                               │
│  ┌─────────────────────────────────────────────────────┐   │
│  │          RuntimeCardDefinition                       │   │
│  │          • CardBehaviour[]                           │   │
│  │            - BehaviourDamageHealth                   │   │
│  │            - BehaviourCombat (future)                │   │
│  └─────────────────────────────────────────────────────┘   │
└───────────────────────────────────────────────────────────────┘
          ▲
          │ loads from
          │
┌─────────┴───────────────────────────────────────────────────┐
│                  CONFIGURATION LAYER                         │
│  ┌────────────────────────────────────────────────────┐     │
│  │  ScriptableObject Data Sources                     │     │
│  │  • SOPlayerProfile                                 │     │
│  │  • SOCardDefinition                                │     │
│  │  • CardGameConfig                                  │     │
│  │  • ICardGameDataManager (Local/Network)            │     │
│  └────────────────────────────────────────────────────┘     │
└─────────────────────────────────────────────────────────────┘
```

### Architectural Layers

#### 1. **Presentation Layer**
- **Responsibility**: Display game state, capture user input
- **Key Classes**: `CardGameViewModel`, `UICard`
- **Communication**: One-way subscription to GameEvents
- **Pattern**: MVVM variant (passive view)

#### 2. **Event Bus Layer**
- **Responsibility**: Decoupled communication between systems
- **Key Classes**: `GameEvents` (static), `Signal<T>` (generic event wrapper)
- **Pattern**: Observer/Publisher-Subscriber
- **Advantages**: Zero coupling between publishers and subscribers

#### 3. **Game Logic Layer**
- **Responsibility**: Core gameplay rules, state management, action processing
- **Key Classes**: `CardGameManager`, `CardGameController`, `GameStateMachine`
- **Pattern**: Facade + State Machine + Command Pattern
- **Threading**: Single-threaded with async initialization support

#### 4. **Data/State Layer**
- **Responsibility**: Runtime game state storage
- **Key Classes**: `PlayerState`, `MatchState`, `RuntimeCardDefinition`
- **Pattern**: Component-based architecture
- **Mutability**: Mutable state managed through controlled interfaces

#### 5. **Configuration Layer**
- **Responsibility**: Static game definitions, data loading
- **Key Classes**: ScriptableObjects (`SOPlayerProfile`, `SOCardDefinition`)
- **Pattern**: Repository + Adapter
- **Flexibility**: Swappable data sources (local/network)

---

## Core Systems

### 1. State Machine System

#### Purpose
Controls the game's lifecycle and enforces valid state transitions.

#### States

| State | Entry Condition | Exit Condition | Responsibilities |
|-------|----------------|----------------|------------------|
| **Initializing** | Game start | Data loaded | Load match data, instantiate players |
| **InGame** | Initialization complete | Pause input / Game over | Process actions, update timer, handle gameplay |
| **Paused** | Pause input during InGame | Resume input | Freeze game time, display pause UI |
| **GameOver** | Victory/defeat condition | Manual restart | Display results, cleanup |

#### State Transition Flow
```
[Start] → Initializing → InGame ⇄ Paused
                            ↓
                        GameOver → [End]
```

#### Implementation Details
```csharp
public interface IGameState
{
    InGameStateId StateId { get; }
    void OnEnter(InGameStateId lastState);
    InGameStateId OnUpdate();  // Returns next state or None
    void OnExit(InGameStateId nextState);
}
```

**Key Design Decision**: States return the next state ID rather than triggering transitions directly. This keeps transition logic centralized in `GameStateMachine`.

### 2. Action System (Command Pattern)

#### Architecture
```
User Input → IGameAction → ActionHandlersManager → ActionHandler<T> → CardGameController
```

#### Components

**IGameAction Interface**
```csharp
public interface IGameAction
{
    Guid PlayerGuid { get; }
}
```
- Simple marker interface requiring player identification
- Extensible: add new actions without modifying existing code

**ActionHandler<T> Abstract Class**
```csharp
public abstract class ActionHandler<T> : IActionHandler where T : IGameAction
{
    Type IActionHandler.ActionType => typeof(T);
    void IActionHandler.Execute(IGameAction action);
    public abstract void Execute(T action);  // Type-safe implementation
}
```
- Provides type safety while maintaining polymorphism
- Double dispatch pattern: interface method casts to specific type

**ActionHandlersManager**
- Registry pattern: `Dictionary<Type, IActionHandler>`
- Delegates actions to appropriate handlers
- Easily extensible: register new handlers in constructor

#### Example Action Flow
```
1. Player clicks "Draw Card" button
2. UI creates: new ActionDrawCard(playerGuid)
3. UI triggers: GameEvents.OnGameActionRequest.Trigger(action)
4. StateInGame (subscribed) receives action
5. ActionHandlersManager routes to ActionHandlerDrawCard
6. Handler executes: cardGameController.TryDrawACard(playerGuid)
7. Controller updates PlayerState
8. Controller triggers: GameEvents.OnCardAdded_Hand
9. UI (subscribed) updates display
```

### 3. Card Behavior System

#### Design Philosophy
Cards are data containers; behaviors are executable strategies applied to targets.

#### Class Hierarchy
```
CardBehaviour (abstract)
├── BehaviourDamageHealth (implemented)
├── BehaviourCombat (stub)
└── [Future behaviors...]

IBehaviourTarget (marker interface)
├── PlayerState
└── RuntimeCardDefinition
```

#### Behavior Execution Flow
```csharp
// Simplified from CardGameController.TryUseCard()
foreach (CardBehaviour behaviour in card.Behaviours)
{
    // 1. Get targeting strategy
    IBehaviourTargetsHandler handler = 
        _behaviourTargetsHandlers[behaviour.TargetType];
    
    // 2. Resolve targets
    List<IBehaviourTarget> targets = 
        handler.GetBehaviourTargets(owner, behaviour, targetGuid);
    
    // 3. Execute behavior
    behaviour.TryActivateBehaviour(owner, targets);
}
```

#### Target Resolution Strategies

| TargetType | Handler | Behavior |
|------------|---------|----------|
| `OwnerPlayer` | `TargetHandlerOwnerPlayer` | Returns card owner only |
| `EnemyPlayers` | `TargetHandlerEnemyPlayers` | Returns all opponents or specific target if Guid provided |

**Extensibility Point**: Add new target types (e.g., `AllPlayers`, `RandomEnemy`, `AdjacentCards`) by implementing `IBehaviourTargetsHandler`.

### 4. Card Set Management

#### CardSetController

Wrapper around `Dictionary<Guid, RuntimeCardDefinition>` with ordered `List<Guid>` for deck ordering.

**Key Methods**:
- `TryAddCard()` / `TryRemoveCard()`: Safe manipulation with validation
- `TryGetFirstCard()` / `TryGetLastCard()`: Deck top/bottom access
- `Shuffle()`: Fisher-Yates algorithm
- `Overwrite()`: Bulk replacement (used for deck initialization)

**Design Decision**: Separate Guid list maintains card order independent of dictionary, enabling efficient shuffling and draw operations.

#### Card Movement Pipeline
```
1. CardGameController.TryDrawACard(playerGuid)
2. Get deck.TryGetFirstCard(out card)
3. MoveCardFromTo(card, deck, hand)
   ├── deck.TryRemoveCard(cardGuid, out card)
   └── hand.TryAddCard(cardGuid, card)
4. Trigger events:
   ├── GameEvents.OnCardRemoved_Deck
   ├── GameEvents.OnCardAdded_Hand
   └── GameEvents.OnCardMoved_DeckToHand
```

### 5. Player Component System

#### PlayerFloatComponent

Generic numeric component with event-driven updates.

**Features**:
- Min/Max clamping
- Safe increment/decrement methods
- Granular event signals:
  - `OnValueChanged`
  - `OnValueIncrease` / `OnValueDecrease`
  - `OnValueIsMax` / `OnValueIsMin`

**Usage Example**:
```csharp
// During PlayerState initialization
PlayerFloatComponent health = new PlayerFloatComponent(
    ownerGuid: player.Guid,
    initialValue: 100,
    minValue: 0,
    maxValue: 100
);

health.OnValueIsMin.AddListener(OnPlayerHealthIsZero);
health.SafeDecreaseValue(25);  // Triggers events, clamps to min
```

**Extensibility**: Create `PlayerIntComponent`, `PlayerBoolComponent`, or custom components following same pattern.

---

## Design Patterns & Principles

### Pattern Implementation Matrix

| Pattern | Location | Purpose | Benefits |
|---------|----------|---------|----------|
| **Singleton** | `CardGameManager` | Single game instance | Global access to game state |
| **Observer** | `Signal<T>`, `GameEvents` | Decoupled communication | Zero coupling between systems |
| **Command** | `IGameAction`, `ActionHandler<T>` | Encapsulate actions | Undo/redo potential, logging |
| **State** | `GameStateMachine`, `IGameState` | Manage game flow | Clear state transitions |
| **Strategy** | `IBehaviourTargetsHandler` | Pluggable targeting | Easy to add target types |
| **Factory** | `RuntimeCardFactory` | Create runtime objects | Centralized creation logic |
| **Template Method** | `ActionHandler<T>` | Type-safe execution | Polymorphism + type safety |
| **Facade** | `CardGameController` | Simplify complex operations | Hide implementation details |
| **Repository** | `ICardGameDataManager` | Abstract data source | Swap local/network data |
| **Component** | `PlayerFloatComponent` | Modular attributes | Flexible composition |
| **Dependency Injection** | Constructor parameters | Loose coupling | Testability |

### SOLID Principles Adherence

#### Single Responsibility Principle (SRP)
✅ **Well Applied**:
- `CardGameManager`: Only orchestrates, doesn't implement logic
- `CardGameController`: Only manipulates game state
- `CardSetController`: Only manages card collections
- Each action handler: Only processes one action type

#### Open/Closed Principle (OCP)
✅ **Well Applied**:
- New card behaviors: Create new `CardBehaviour` subclass
- New actions: Create `IGameAction` + `ActionHandler<T>`
- New target types: Implement `IBehaviourTargetsHandler`
- No modification of existing code required

#### Liskov Substitution Principle (LSP)
✅ **Well Applied**:
- All `IGameState` implementations interchangeable
- All `ActionHandler<T>` work through `IActionHandler` interface
- `PlayerController` and `AIController` both extend `Controller`

#### Interface Segregation Principle (ISP)
✅ **Well Applied**:
- Marker interfaces: `IGameAction`, `IBehaviourTarget`, `IComponent`
- Small, focused interfaces prevent unnecessary dependencies

⚠️ **Could Improve**:
- `ICardGameDataManager` currently has single method, but may grow

#### Dependency Inversion Principle (DIP)
✅ **Well Applied**:
- High-level `CardGameController` depends on abstractions (`IGameAction`)
- Data source abstraction (`ICardGameDataManager`)
- No direct MonoBehaviour dependencies in logic classes

---

## Data Flow Architecture

### Initialization Flow

```
1. Unity Scene Loads
   ↓
2. CardGameManager.Awake()
   ├── Instantiate CardGameController
   ├── Instantiate MatchState
   └── Create GameStateMachine
   ↓
3. GameStateMachine.StartMachine(Initializing)
   ↓
4. StateInitializing.OnEnter()
   ├── CardGameManager.SetupMatch() [async]
   ├── Load match data (ICardGameDataManager)
   └── SetupPlayers()
       ├── Instantiate PlayerControllers
       ├── Create PlayerStates
       └── Trigger GameEvents.OnNewPlayerAdded
   ↓
5. StateInitializing.OnUpdate() → returns InGame
   ↓
6. StateInGame.OnEnter()
   ├── FillPlayersDecks()
   ├── ShufflePlayersDecks()
   └── TryFillPlayersHands()
       └── Triggers OnCardAdded_Hand events
   ↓
7. UI subscribes to events, updates display
```

### Gameplay Action Flow

```
User Input → Event → State → Action Handler → Controller → State Update → Event → UI
```

**Detailed Example: Using a Damage Card**

```
1. USER INTERACTION
   Player clicks UICard.TEST_Use()
   ↓
2. ACTION CREATION
   new ActionUseCard(ownerGuid, cardGuid, targetGuid)
   ↓
3. EVENT PUBLICATION
   GameEvents.OnGameActionRequest.Trigger(action)
   ↓
4. STATE PROCESSING
   StateInGame receives action (subscribed)
   → ActionHandlersManager.HandleActionRequest(action)
   ↓
5. HANDLER ROUTING
   Manager finds ActionHandlerUseCard by action type
   → ActionHandlerUseCard.Execute(action)
   ↓
6. CONTROLLER EXECUTION
   CardGameController.TryUseCard(ownerGuid, cardGuid, targetGuid)
   ├── Get card from player's hand
   ├── For each CardBehaviour in card:
   │   ├── Get IBehaviourTargetsHandler
   │   ├── Resolve targets
   │   └── behaviour.TryActivateBehaviour(owner, targets)
   │       └── BehaviourDamageHealth.TryActivateBehaviour()
   │           └── playerState.Health.SafeDecreaseValue(damage)
   │               └── Triggers OnPlayerHealthChanged event
   └── CardFromHandToDiscarded(card)
       ├── Triggers OnCardRemoved_Hand
       └── Triggers OnCardAdded_Discarded
   ↓
7. EVENT SUBSCRIBERS
   CardGameViewModel.OnPlayerHPChanged(ownerGuid, newHP)
   → Updates UI text: _playerHealth.SetText(newHP)
```

### Data Transformation Pipeline

```
Editor-Time Data → Runtime Data → Game State

SOCardDefinition              RuntimeCardDefinition
(ScriptableObject)            (Runtime instance)
├── CardName                  ├── Guid (generated)
├── Description               ├── CardName
└── CardBehaviourData[]       ├── Description
                              └── CardBehaviour[] (instantiated)
        ↓
RuntimeCardFactory.TryCreateRuntimeCardDefinition()
        ↓
SOPlayerProfile               RuntimePlayerProfile
├── PlayerName                ├── Guid (generated)
├── PlayerType                ├── PlayerName
├── ComponentsData            ├── PlayerType
└── SOCardDefinition[]        ├── ComponentsData
                              └── Dictionary<Guid, RuntimeCard>
        ↓
CardGameManager.SetupPlayers()
        ↓
                              PlayerState
                              ├── RuntimePlayerGuid
                              ├── PlayerName
                              ├── Components (Health, Mana)
                              └── CardSets (Deck, Hand, InGame, Discarded)
```

---

## Component Deep Dive

### CardGameManager (Orchestrator)

**Responsibilities**:
1. Bootstrap game systems
2. Maintain references to all major components
3. Provide read-only access to game state
4. Manage player lifecycle

**Key Design Decisions**:
- Singleton for global access (acceptable for game manager)
- Read-only dictionaries exposed (`IReadOnlyDictionary`)
- No direct state manipulation (delegates to Controller)

**Initialization Sequence**:
```csharp
void Awake()
{
    ValidateConfig();
    _cardGameController = new CardGameController(this);
    _matchState = new MatchState();
    _stateMachine = new GameStateMachine(stateDictionary);
    _stateMachine.StartMachine(InGameStateId.Initializing);
}
```

### CardGameController (Facade)

**Responsibilities**:
1. Encapsulate complex card operations
2. Enforce game rules
3. Trigger appropriate events
4. Validate state before mutations

**Method Naming Convention**:
- `Try*` prefix: Returns bool, non-throwing
- Private helpers: No prefix (e.g., `MoveCardFromTo`)

**Error Handling Philosophy**:
```csharp
public bool TryDrawACard(Guid playerGuid)
{
    // 1. Validate inputs
    if (!_manager.TryGetPlayerState(playerGuid, out var state))
    {
        Debug.LogError("Invalid player");
        return false;
    }
    
    // 2. Check preconditions
    if (deck.Count == 0)
    {
        Debug.Log("Deck empty");  // Not an error
        return false;
    }
    
    // 3. Execute operation
    if (!MoveCardFromTo(card, deck, hand))
    {
        Debug.LogError("Move failed");
        return false;
    }
    
    // 4. Trigger events
    GameEvents.OnCardAdded_Hand.Trigger(playerGuid, card);
    return true;
}
```

### RuntimeCardDefinition vs CardDefinitionData

**Separation of Concerns**:

| Type | Purpose | Lifetime | Mutability |
|------|---------|----------|------------|
| `CardDefinitionData` (struct) | Editor-time definition | Permanent (asset) | Immutable |
| `RuntimeCardDefinition` (class) | Runtime instance | Match duration | Mutable (future) |

**Why Separate?**:
1. **Guid Assignment**: Runtime cards get unique identifiers
2. **Stat Modifications**: Future support for buffs/debuffs
3. **Memory Efficiency**: Share static data, instance dynamic data
4. **Save/Load**: Serialize runtime state separately from definitions

### PlayerState vs RuntimePlayerProfile

| Aspect | RuntimePlayerProfile | PlayerState |
|--------|---------------------|-------------|
| **Purpose** | Initial configuration | Active game state |
| **Creation** | Bootstrap/lobby | Match start |
| **Contains** | Deck definition, defaults | Live health, card sets |
| **Mutability** | Immutable after creation | Constantly changing |
| **Persistence** | Could be saved to disk | Match-only (could be serialized) |

---

## Extensibility & Scalability

### Adding New Card Behaviors

**Step-by-Step**:

1. **Define Behavior Type**
```csharp
// In Enums.cs
public enum BehaviourType
{
    DamageHealth,
    RecoverHealth,  // NEW
    DrawCards       // NEW
}
```

2. **Create Behavior Class**
```csharp
public class BehaviourRecoverHealth : CardBehaviour
{
    public BehaviourRecoverHealth(string name, float amount, TargetType target)
        : base(name, amount, target) { }
    
    public override bool TryActivateBehaviour(
        PlayerState owner, 
        List<IBehaviourTarget> targets)
    {
        foreach (IBehaviourTarget target in targets)
        {
            if (target is PlayerState player)
            {
                player.Health.SafeIncreaseValue(_mainValue);
            }
        }
        return true;
    }
}
```

3. **Update Factory**
```csharp
// In RuntimeCardFactory.cs
public static bool TryCreateRuntimeEffect(
    CardBehaviourData data, 
    out CardBehaviour behaviour)
{
    switch (data.BehaviourType)
    {
        case BehaviourType.DamageHealth:
            behaviour = new BehaviourDamageHealth(...);
            return true;
        
        case BehaviourType.RecoverHealth:  // NEW
            behaviour = new BehaviourRecoverHealth(...);
            return true;
        
        // ...
    }
}
```

4. **Create ScriptableObject**
- In Unity Editor: Create → Card Definition
- Add behavior with type `RecoverHealth`
- Add to player deck

**No other code changes required!**

### Adding New Target Types

**Example: "AllPlayers" targeting**

1. **Add Enum**
```csharp
public enum TargetType
{
    OwnerPlayer,
    EnemyPlayers,
    AllPlayers  // NEW
}
```

2. **Implement Handler**
```csharp
public class TargetHandlerAllPlayers : IBehaviourTargetsHandler
{
    private readonly CardGameManager _manager;
    
    public TargetHandlerAllPlayers(CardGameManager manager)
    {
        _manager = manager;
    }
    
    public List<IBehaviourTarget> GetBehaviourTargets(
        PlayerState owner,
        CardBehaviour behaviour,
        Guid targetGuid = default)
    {
        return _manager.PlayerStates.Values
            .Cast<IBehaviourTarget>()
            .ToList();
    }
}
```

3. **Register Handler**
```csharp
// In CardGameController constructor
_behaviourTargetsHandlers = new Dictionary<TargetType, IBehaviourTargetsHandler>
{
    { TargetType.OwnerPlayer, new TargetHandlerOwnerPlayer() },
    { TargetType.EnemyPlayers, new TargetHandlerEnemyPlayers(_manager) },
    { TargetType.AllPlayers, new TargetHandlerAllPlayers(_manager) }  // NEW
};
```

### Network Multiplayer Considerations

**Current Architecture Supports**:
- ✅ Swappable data source (`ICardGameDataManager`)
- ✅ Command pattern for action serialization
- ✅ Guid-based entity identification
- ✅ Event-driven updates (can be networked)

**Required Additions**:
1. **Network Transport Layer**
   - Implement `NetworkDataManager : ICardGameDataManager`
   - Serialize/deserialize `IGameAction` objects

2. **Authority System**
   - Add `bool IsLocalPlayer` to `Controller`
   - Validate actions on server before execution

3. **State Synchronization**
   - Snapshot `PlayerState` periodically
   - Reconcile client predictions with server state

4. **Action Validation**
```csharp
public class NetworkActionValidator
{
    public bool ValidateAction(IGameAction action, PlayerState state)
    {
        // Check if player can afford action
        // Validate card is in hand
        // Verify it's player's turn
        return isValid;
    }
}
```

**Recommended Architecture**:
```
Client                         Server
  │                              │
  ├──(1) User Input              │
  ├──(2) Optimistic Update       │
  ├──(3) Send Action ────────────→
  │                              ├──(4) Validate Action
  │                              ├──(5) Execute Action
  │                              ├──(6) Broadcast State
  ├──(7) Receive State ←─────────┘
  └──(8) Reconcile Prediction
```

### AI System Extension

**Current AI Controller**: Stub implementation

**Recommended AI Architecture**:

```csharp
public class AIController : Controller
{
    private IAIStrategy _strategy;
    private float _decisionInterval = 1f;
    
    public override void Initialize(Guid playerGuid)
    {
        base.Initialize(playerGuid);
        _strategy = new AggressiveStrategy();  // or DefensiveStrategy
    }
    
    void Update()
    {
        if (!IsMyTurn()) return;
        
        if (Time.time > _nextDecisionTime)
        {
            IGameAction action = _strategy.DecideAction(
                GetPlayerState(),
                GetGameState()
            );
            
            if (action != null)
            {
                GameEvents.OnGameActionRequest.Trigger(action);
            }
            
            _nextDecisionTime = Time.time + _decisionInterval;
        }
    }
}

public interface IAIStrategy
{
    IGameAction DecideAction(PlayerState self, CardGameManager game);
}

public class AggressiveStrategy : IAIStrategy
{
    public IGameAction DecideAction(PlayerState self, CardGameManager game)
    {
        // 1. Find highest damage card
        // 2. Target enemy with lowest health
        // 3. Return ActionUseCard
    }
}
```

---

## Technical Debt & Considerations

### Current Limitations

#### 1. **Synchronous Card Operations**
**Issue**: All card movements are immediate
```csharp
// Current: Instant
CardFromDeckToHand(card, player);
GameEvents.OnCardAdded_Hand.Trigger(playerGuid, card);

// Better: Async with animation support
await AnimateCardMovement(card, deck, hand);
GameEvents.OnCardAdded_Hand.Trigger(playerGuid, card);
```

**Impact**: UI animations must be fire-and-forget
**Solution**: Introduce `async Task` methods with cancellation tokens

#### 2. **No Turn System**
**Issue**: Players can act simultaneously
**Impact**: Race conditions, unclear game flow
**Solution**: Add `TurnManager` component
