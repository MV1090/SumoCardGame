# Sumo Card Game - Game Design Document

## Overview

**Sumo Card Game** is an online multiplayer card game for 2 players. Players compete in a series of sumo wrestling bouts using a deck of cards to strategically manage their sumo wrestler's stamina, strength, and abilities.

## Technology Stack

- **Engine**: Unity 6
- **Networking**: Unity Netcode for GameObjects
- **Multiplayer Services**: Unity Multiplayer Services
- **Rendering**: Universal Render Pipeline (URP) 2D

## Matchmaking

Players can join games in two ways:
- **Random Matchmaking**: Join a random game with another player
- **Private Match**: Use a designated code to join a game with a friend

## Game Flow

### Phase 1: Lobby & Setup

1. **Host Configuration**
   - The host player sets up the game rules:
     - **Number of Sumo Cards**: Determines how many bouts will be played (e.g., 3 sumos = 3 bouts)
     - **Sumo Selection Method**: 
       - Random selection
       - Selected by previous winner
       - Coin toss

2. **Player Joining**
   - Players join the game in the lobby menu
   - Both players must set their status to **'Ready'** before the game can start

### Phase 2: Sumo Selection

Once both players are ready:
1. Each player takes turns selecting their Sumo cards for the game (based on the number set in the rules)
2. Once both players have selected their required number of Sumo cards, they choose which Sumo they will use first
3. Both players draw **5 cards** from their deck - this becomes their starting hand

### Phase 3: Round Structure

#### Visual Setup
- Both Sumos are displayed on screen
- Player's own hand: **Face up** (visible)
- Opponent's hand: **Face down** (hidden)

#### Round Play Sequence

1. **Card Playing Phase**
   - Both players simultaneously play:
     - **1 Wrestling Card**
     - **1 Utility Card**
   - Cards are **hidden from the opponent** until both players end their turn

2. **Resolution Phase** (after both players end turn)
   - **Utility Cards are resolved first**
     - Resolution order based on:
       1. Card's execution order (if specified)
       2. If execution order is the same: **Sumo's current stamina** (higher stamina resolves first)
   - Some utility cards have unique rules and can be used **before the round starts**
   - **Wrestling Cards are resolved next**
     - These cards affect how much stamina the Sumos lose

3. **Cleanup Phase**
   - **Passive/Persistent Utility Cards**: Remain in play until removed by another card
   - **Instant Utility Cards**: Discarded
   - **All Wrestling Cards**: Discarded
   - **Card Drawing Phase**:
     - Players take turns drawing cards until their hand size equals their current hand size
     - Drawing order: **Sumo with more stamina draws first**

### Phase 4: Victory Conditions

- A Sumo is **defeated** when pushed out of the ring
- **Ring-Out Mechanic** (to be fully implemented):
  - Based on:
    - Sumo's **weight**
    - Sumo's **remaining stamina**
  - Should include an **element of chance** for the Sumo to stay in the ring

### Phase 5: Bout System

1. When one Sumo is defeated:
   - Both players discard their current Sumo card
   - Players select their next Sumo for the next bout
   - Process repeats until both players are out of Sumos

2. **Match Winner**: Determined by who won the most bouts

### Phase 6: Post-Game

- Players can choose to:
  - **Play Again**: Start a new game from the beginning
  - **Leave Game**: Exit to menu

## Card Types

### Sumo Cards
- Each player selects a number of Sumo cards at the start of the game
- Sumo cards have:
  - **Stamina**: Health/endurance stat
  - **Weight**: Affects ring-out mechanics
  - **Abilities**: Unique sumo abilities

### Wrestling Cards
- Played once per round
- Affect stamina loss
- Discarded after resolution

### Utility Cards
- Played once per round
- Three types:
  - **Instant**: Resolved and discarded immediately
  - **Passive**: Remains in play until removed
  - **Persistent**: Remains in play until removed
- Some utility cards can be used before the round starts

## Player Stats

Players have the following stats that can be modified by cards:
- **Stamina**: Current health/endurance
- **Strength**: Affects combat effectiveness
- **Defense**: Reduces incoming damage
- **Weight**: Affects ring-out mechanics
- **Hand Size**: Maximum number of cards in hand
- **Cards to Draw**: Number of cards drawn per turn

## Future Development

- **Abilities System**: Still being designed and planned
- **Card Library**: Additional cards and abilities to be added
- **Ring-Out Mechanic**: Full implementation pending
- **Visual Polish**: UI/UX improvements
- **Sound & Effects**: Audio and visual feedback

## Notes

- This document serves as a living reference and will be updated as the game design evolves
- Card abilities and specific mechanics are subject to change during development
- Balance adjustments will be made based on playtesting feedback

