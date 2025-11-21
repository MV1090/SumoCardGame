# Sumo Card Game - Implementation Plan

This document outlines the step-by-step implementation plan to transform the current single-player prototype into a fully functional online multiplayer card game as described in the GDD.

## Current State Assessment

### ✅ Already Implemented
- Basic card ScriptableObject system (Sumo, Wrestling, Utility cards)
- Deck building and shuffling system
- Hand management (basic card display and positioning)
- Card drawing mechanics
- Player stats system (stamina, strength, defense, weight, hand size)
- Card visual display system (BaseCard, SumoCard, UtilityCard, WrestlingCard)
- Basic ability framework (structure exists, not implemented)
- Editor helpers (ReadOnly attribute, CardSizeManagement)

### ❌ Missing/Needs Implementation
- Multiplayer/Networking infrastructure
- Lobby system
- Matchmaking (random/private)
- Game state management
- Round-based gameplay
- Card playing mechanics
- Resolution system
- Victory conditions
- Bout system
- UI/UX for all phases

---

## Implementation Sections

### Section 1: Networking Foundation
**Goal**: Set up the core networking infrastructure using Unity Netcode for GameObjects

#### 1.1 Network Setup & Configuration
- [x] Create NetworkManager prefab and configure Netcode settings
- [x] Set up NetworkObject components on key game objects
- [x] Configure network prefabs list
- [ ] Create network scene management system
- [ ] Set up client-server architecture (host/client model)
- [ ] Test basic connection between two clients

#### 1.2 Network Player System
- [ ] Convert Player class to NetworkBehaviour
- [ ] Create NetworkPlayerData scriptable object for player info
- [ ] Implement player ownership and authority checks
- [ ] Create player connection/disconnection handlers
- [ ] Set up player ID system (host vs client)
- [ ] Implement network synchronization for player stats

#### 1.3 Network Card System
- [ ] Convert CardDeck to NetworkBehaviour
- [ ] Make card GameObjects network objects
- [ ] Implement network synchronization for deck state
- [ ] Create network-safe card drawing (server-authoritative)
- [ ] Implement network hand synchronization
- [ ] Add network visibility controls (hide opponent cards)

---

### Section 2: Lobby & Matchmaking System
**Goal**: Create the lobby system where players join and configure game rules

#### 2.1 Lobby UI Foundation
- [ ] Create Lobby scene
- [ ] Design and implement main menu UI (Start Game, Join Game buttons)
- [ ] Create lobby UI layout (player slots, ready buttons, settings)
- [ ] Implement UI state management system
- [ ] Add loading/connecting indicators

#### 2.2 Matchmaking System
- [ ] Implement Unity Multiplayer Services integration
- [ ] Create random matchmaking system
- [ ] Create private match system (code-based joining)
- [ ] Implement lobby code generation and validation
- [ ] Add matchmaking UI (find match, enter code)
- [ ] Handle matchmaking errors and timeouts

#### 2.3 Game Rules Configuration
- [ ] Create GameRules ScriptableObject/NetworkVariable
- [ ] Implement host-only rule configuration UI
  - [ ] Number of Sumo cards selector (1-5 range)
  - [ ] Sumo selection method dropdown (Random, Winner Chooses, Coin Toss)
- [ ] Sync game rules to all clients
- [ ] Add rule display for non-host players (read-only)
- [ ] Validate rules before game start

#### 2.4 Player Ready System
- [ ] Create ready status NetworkVariable per player
- [ ] Implement ready/unready button functionality
- [ ] Add visual feedback for ready status (checkmarks, colors)
- [ ] Create ready status synchronization
- [ ] Implement game start validation (both players ready + rules set)
- [ ] Add countdown timer before game start (optional)

---

### Section 3: Sumo Selection Phase
**Goal**: Implement the phase where players select their Sumos for the match

#### 3.1 Sumo Selection UI
- [ ] Create Sumo Selection scene/UI
- [ ] Design Sumo card selection interface
- [ ] Implement Sumo card display (grid/list view)
- [ ] Add selection highlighting and confirmation
- [ ] Create "Select First Sumo" UI after all Sumos chosen
- [ ] Add turn indicator (whose turn to select)

#### 3.2 Sumo Selection Logic
- [ ] Create SumoSelectionManager (NetworkBehaviour)
- [ ] Implement turn-based selection system
- [ ] Store selected Sumos per player (NetworkList)
- [ ] Enforce selection rules (number of Sumos based on game rules)
- [ ] Implement selection method logic:
  - [ ] Random selection
  - [ ] Winner chooses (for subsequent bouts)
  - [ ] Coin toss
- [ ] Validate selections before proceeding

#### 3.3 First Sumo Selection
- [ ] Create first Sumo selection phase
- [ ] Allow players to choose which Sumo to use first
- [ ] Sync first Sumo selection to all clients
- [ ] Initialize player stats from selected Sumo
- [ ] Transition to game scene

#### 3.4 Initial Hand Setup
- [ ] Implement initial 5-card draw after Sumo selection
- [ ] Ensure deck is built and shuffled before draw
- [ ] Sync initial hand state
- [ ] Verify hand size matches (5 cards each)

---

### Section 4: Game State Management
**Goal**: Create a robust game state system to manage all phases of gameplay

#### 4.1 Game State System
- [ ] Create GameStateManager (NetworkBehaviour, singleton)
- [ ] Define GameState enum (Lobby, SumoSelection, RoundSetup, Playing, Resolution, Cleanup, BoutEnd, MatchEnd)
- [ ] Implement state machine with transitions
- [ ] Add NetworkVariable for current game state
- [ ] Create state change events/notifications
- [ ] Implement state validation and error handling

#### 4.2 Round Management
- [ ] Create RoundManager (NetworkBehaviour)
- [ ] Track current round number
- [ ] Track current bout number
- [ ] Implement round start/end logic
- [ ] Manage turn phases within rounds
- [ ] Sync round state to all clients

#### 4.3 Player Action System
- [ ] Create PlayerActionManager (NetworkBehaviour)
- [ ] Implement action queue system
- [ ] Create action validation (can player do this?)
- [ ] Add action confirmation system
- [ ] Implement action synchronization
- [ ] Handle action rollback on errors

---

### Section 5: Card Playing System
**Goal**: Implement the core card playing mechanics with hidden cards until both players ready

#### 5.1 Card Playing UI
- [ ] Create card playing interface
- [ ] Add card selection/drag-drop system
- [ ] Implement play zones (Wrestling card zone, Utility card zone)
- [ ] Add "End Turn" button
- [ ] Create card preview/hover effects
- [ ] Add visual feedback for played cards (face down for opponent)

#### 5.2 Card Playing Logic
- [ ] Create PlayedCard data structure (NetworkSerializable)
- [ ] Implement card playing validation:
  - [ ] Must play exactly 1 Wrestling card
  - [ ] Must play exactly 1 Utility card
  - [ ] Cards must be in hand
- [ ] Store played cards per player (NetworkList)
- [ ] Hide opponent's played cards until both ready
- [ ] Implement "End Turn" functionality
- [ ] Add turn state synchronization

#### 5.3 Pre-Round Utility Cards
- [ ] Identify utility cards that can be played before round
- [ ] Create pre-round card playing phase
- [ ] Add UI for pre-round card selection
- [ ] Implement pre-round card resolution
- [ ] Integrate with main round flow

#### 5.4 Card Visibility System
- [ ] Implement card face-up/face-down rendering
- [ ] Create card back sprite/material
- [ ] Add network visibility controls
- [ ] Sync card reveal timing (when both players ready)
- [ ] Add reveal animation/effect

---

### Section 6: Resolution System
**Goal**: Implement the card resolution system with proper ordering

#### 6.1 Resolution Manager
- [ ] Create ResolutionManager (NetworkBehaviour)
- [ ] Implement resolution phase trigger (both players ended turn)
- [ ] Create resolution queue system
- [ ] Add resolution state tracking
- [ ] Implement resolution completion detection

#### 6.2 Utility Card Resolution
- [ ] Sort utility cards by execution order
- [ ] Implement stamina-based tiebreaker (higher stamina resolves first)
- [ ] Create utility card resolution queue
- [ ] Execute utility cards in order
- [ ] Handle resolution errors gracefully
- [ ] Add resolution visual feedback

#### 6.3 Wrestling Card Resolution
- [ ] Create wrestling card resolution system
- [ ] Calculate stamina loss from wrestling cards
- [ ] Apply wrestling card effects to Sumos
- [ ] Update Sumo stamina display
- [ ] Add combat visual feedback
- [ ] Handle simultaneous card resolution

#### 6.4 Ability Execution
- [ ] Implement BasicAbility_Scriptable.ActivateAbility() logic
- [ ] Create ability effect system:
  - [ ] StrengthBonus/DefenseBonus application
  - [ ] HealBonus application
  - [ ] HandSizeBonus application
  - [ ] WeightBonus application
  - [ ] Opponent stat reduction effects
- [ ] Add ability duration tracking
- [ ] Implement ability removal system
- [ ] Create unique ability framework for future expansion

---

### Section 7: Cleanup Phase
**Goal**: Implement the cleanup phase after each round

#### 7.1 Card Cleanup System
- [ ] Create CleanupManager (NetworkBehaviour)
- [ ] Identify card types (Instant, Passive, Persistent)
- [ ] Implement card cleanup logic:
  - [ ] Discard Instant utility cards
  - [ ] Keep Passive/Persistent utility cards in play
  - [ ] Discard all wrestling cards
- [ ] Create in-play card tracking system
- [ ] Add visual feedback for card removal

#### 7.2 Card Drawing Phase
- [ ] Calculate drawing order (Sumo with more stamina draws first)
- [ ] Implement turn-based drawing system
- [ ] Draw until hand size equals current hand size
- [ ] Handle deck exhaustion (shuffle discard pile if needed)
- [ ] Sync drawing state to all clients
- [ ] Add drawing animation/feedback

#### 7.3 In-Play Card Management
- [ ] Create InPlayCardManager (NetworkBehaviour)
- [ ] Track active Passive/Persistent cards per player
- [ ] Display in-play cards on screen
- [ ] Implement card removal triggers
- [ ] Update card effects when removed
- [ ] Add visual indicators for active cards

---

### Section 8: Victory Conditions & Ring-Out System
**Goal**: Implement the ring-out mechanic and victory detection

#### 8.1 Ring-Out Calculation
- [ ] Create RingOutCalculator class
- [ ] Implement ring-out formula:
  - [ ] Base calculation on weight and stamina
  - [ ] Add random chance element
  - [ ] Factor in card effects (if any affect ring-out)
- [ ] Test and balance ring-out mechanics
- [ ] Add ring-out probability display (optional)

#### 8.2 Victory Detection
- [ ] Create VictoryManager (NetworkBehaviour)
- [ ] Check for ring-out after each round
- [ ] Detect when Sumo stamina reaches 0 (alternative defeat)
- [ ] Announce round winner
- [ ] Track bout wins per player
- [ ] Detect match end (all Sumos used)

#### 8.3 Victory UI & Feedback
- [ ] Create victory/defeat UI panels
- [ ] Add round winner announcement
- [ ] Display bout score
- [ ] Add match winner screen
- [ ] Implement victory animations/effects
- [ ] Add sound effects for victories

---

### Section 9: Bout System
**Goal**: Implement the multi-bout system where players use multiple Sumos

#### 9.1 Bout Management
- [ ] Create BoutManager (NetworkBehaviour)
- [ ] Track current bout number
- [ ] Track bout wins per player
- [ ] Implement bout end detection
- [ ] Handle Sumo discard after bout
- [ ] Transition to next bout selection

#### 9.2 Next Sumo Selection
- [ ] Reuse Sumo selection UI for subsequent bouts
- [ ] Filter out already-used Sumos
- [ ] Implement selection based on game rules (winner chooses, etc.)
- [ ] Sync next Sumo selection
- [ ] Initialize new Sumo stats
- [ ] Reset game state for new bout

#### 9.3 Match End & Scoring
- [ ] Detect when all Sumos are used
- [ ] Calculate final match winner (most bout wins)
- [ ] Handle tie scenarios (if applicable)
- [ ] Display final match results
- [ ] Track match statistics

---

### Section 10: Post-Game & Rematch
**Goal**: Implement post-game options and rematch functionality

#### 10.1 Post-Game UI
- [ ] Create post-game results screen
- [ ] Display match statistics:
  - [ ] Bout wins/losses
  - [ ] Total rounds played
  - [ ] Cards played
- [ ] Add "Play Again" button
- [ ] Add "Leave Game" button
- [ ] Implement UI state management

#### 10.2 Rematch System
- [ ] Implement "Play Again" functionality
- [ ] Reset game state for new match
- [ ] Keep same players in lobby
- [ ] Reset all game variables
- [ ] Return to lobby/Sumo selection
- [ ] Handle player disconnection during rematch

#### 10.3 Game Exit
- [ ] Implement "Leave Game" functionality
- [ ] Handle graceful disconnection
- [ ] Return to main menu
- [ ] Clean up network objects
- [ ] Reset local game state

---

### Section 11: UI/UX Polish
**Goal**: Create polished, user-friendly interfaces for all game phases

#### 11.1 Main Menu UI
- [ ] Design main menu layout
- [ ] Add game title/logo
- [ ] Implement button animations
- [ ] Add settings menu
- [ ] Create tutorial/help system (optional)

#### 11.2 In-Game UI
- [ ] Create HUD layout:
  - [ ] Player stats display (stamina, strength, defense, weight)
  - [ ] Opponent stats display
  - [ ] Round/Bout counter
  - [ ] Turn indicator
- [ ] Add card hover tooltips
- [ ] Implement card animations (draw, play, discard)
- [ ] Add visual feedback for all actions
- [ ] Create status effect indicators

#### 11.3 Visual Feedback
- [ ] Add card play animations
- [ ] Implement damage/stamina change animations
- [ ] Create card reveal effects
- [ ] Add round transition animations
- [ ] Implement victory/defeat effects
- [ ] Add particle effects for abilities

#### 11.4 Audio System
- [ ] Set up audio manager
- [ ] Add background music
- [ ] Implement sound effects:
  - [ ] Card play sounds
  - [ ] Card draw sounds
  - [ ] Ability activation sounds
  - [ ] Victory/defeat sounds
- [ ] Add UI interaction sounds
- [ ] Implement audio settings

---

### Section 12: Testing & Polish
**Goal**: Ensure the game is stable, balanced, and ready for play

#### 12.1 Network Testing
- [ ] Test connection stability
- [ ] Test reconnection handling
- [ ] Test with poor network conditions
- [ ] Verify synchronization accuracy
- [ ] Test edge cases (disconnects, timeouts)

#### 12.2 Gameplay Testing
- [ ] Test all game phases
- [ ] Verify card resolution order
- [ ] Test ring-out calculations
- [ ] Balance card effects
- [ ] Test all ability types
- [ ] Verify bout system works correctly

#### 12.3 Bug Fixes & Optimization
- [ ] Fix identified bugs
- [ ] Optimize network traffic
- [ ] Optimize UI performance
- [ ] Reduce build size
- [ ] Improve loading times

#### 12.4 Documentation
- [ ] Update GDD with final mechanics
- [ ] Create code documentation
- [ ] Write player guide/tutorial
- [ ] Document network architecture
- [ ] Create developer notes

---

## Implementation Priority

### Phase 1: Core Foundation (Sections 1-2)
**Focus**: Get multiplayer working and players can join games
- Networking Foundation
- Lobby & Matchmaking System

### Phase 2: Game Setup (Section 3)
**Focus**: Players can select Sumos and start a match
- Sumo Selection Phase

### Phase 3: Core Gameplay (Sections 4-7)
**Focus**: Players can play rounds and cards resolve correctly
- Game State Management
- Card Playing System
- Resolution System
- Cleanup Phase

### Phase 4: Victory & Progression (Sections 8-9)
**Focus**: Complete matches with multiple bouts
- Victory Conditions & Ring-Out System
- Bout System

### Phase 5: Polish & Completion (Sections 10-12)
**Focus**: Polish the experience and fix issues
- Post-Game & Rematch
- UI/UX Polish
- Testing & Polish

---

## Notes

- Each section can be worked on somewhat independently, but dependencies exist (e.g., need networking before lobby)
- Some tasks may need to be split further as implementation progresses
- Testing should occur throughout, not just at the end
- Balance adjustments will be ongoing
- Consider creating a test scene for each major system before integrating

---

## Estimated Complexity

- **Section 1**: High complexity (networking is complex)
- **Section 2**: Medium-High complexity (matchmaking services integration)
- **Section 3**: Medium complexity
- **Section 4**: Medium complexity
- **Section 5**: Medium complexity
- **Section 6**: Medium-High complexity (resolution logic)
- **Section 7**: Low-Medium complexity
- **Section 8**: Medium complexity (balancing required)
- **Section 9**: Low-Medium complexity
- **Section 10**: Low complexity
- **Section 11**: Medium complexity (time-consuming)
- **Section 12**: Ongoing

---

*This plan is a living document and should be updated as implementation progresses and new requirements emerge.*

