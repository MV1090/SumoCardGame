# Lobby UI Implementation Guide

This guide provides step-by-step instructions for implementing the UI state management system and loading/connecting indicators for the lobby system.

## Overview

These systems are **local UI concerns** - they don't need to be synchronized over the network. Each client manages its own UI state and visual indicators based on network data (player count, ready status, connection state), but the UI presentation itself is entirely local.

---

## Part 1: UI State Management System

### Step 1.1: Create the State Enum

- Create a `LobbyUIState` enum with the following values:
  - `Connecting` - Initial state when joining
  - `WaitingForPlayers` - Waiting for second player
  - `ConfiguringRules` - Host configuring rules (host only)
  - `WaitingForReady` - Rules set, waiting for players to ready
  - `AllReady` - Both players ready, can start
  - `Starting` - Game is starting (brief state before transition)

### Step 1.2: Add State Tracking Variable

- Add a private `LobbyUIState currentState` variable in your LobbyMenu script
- Add a method `SetUIState(LobbyUIState newState)` that:
  - Updates `currentState`
  - Calls methods to update UI visibility based on state

### Step 1.3: Create UI Element References

- Identify UI elements that need to show/hide:
  - Loading indicator panel
  - Player slots/panels
  - Ready buttons
  - Rules configuration panel (host only)
  - Start game button (host only)
  - Back button
- Add SerializeField references for these UI elements (GameObjects or specific UI components)

### Step 1.4: Implement State-Based UI Updates

- Create a method `UpdateUIForState(LobbyUIState state)` that:
  - Shows/hides appropriate UI elements for each state
  - Enables/disables buttons based on state
  - Updates text labels if needed
- Use a switch statement or if/else chain to handle each state

### Step 1.5: Determine State from Network Data

- Create a method `EvaluateCurrentState()` that:
  - Checks network connection status
  - Checks player count (`NetworkManager.Singleton.ConnectedClients.Count`)
  - Checks if host (`IsServer`)
  - Checks ready states
  - Checks if rules are configured (if you have rules system)
  - Returns the appropriate `LobbyUIState` based on these conditions

### Step 1.6: Call State Evaluation Periodically

- In `Update()` or via coroutine:
  - Call `EvaluateCurrentState()` periodically (every frame or every 0.1-0.5 seconds)
  - If state changed, call `SetUIState(newState)`
- Alternatively, subscribe to network events (`OnClientConnectedCallback`, etc.) and evaluate state when events fire

### Step 1.7: Handle Host vs Client Differences

- In `UpdateUIForState()`:
  - Rules configuration panel: only show for host
  - Start game button: only show for host
  - Ready button: show for both, but host might have different behavior
- Use `IsServer` or `NetworkManager.Singleton.IsHost` to check

### Step 1.8: Test State Transitions

- Test each state transition:
  - Connecting → WaitingForPlayers (when second player joins)
  - WaitingForPlayers → ConfiguringRules (host only)
  - ConfiguringRules → WaitingForReady (when rules set)
  - WaitingForReady → AllReady (when both ready)
  - AllReady → Starting (when game starts)

---

## Part 2: Loading/Connecting Indicators

### Step 2.1: Create Loading Indicator UI Elements

- Add UI elements to your lobby scene:
  - Loading spinner (Image with rotating sprite, or use Unity's built-in)
  - "Connecting..." text (TMP_Text)
  - "Waiting for players..." text (TMP_Text)
  - Optional: progress bar or animated dots

### Step 2.2: Add References to LobbyMenu

- Add SerializeField references for:
  - Loading indicator GameObject/Image
  - Connecting text (TMP_Text)
  - Waiting text (TMP_Text)

### Step 2.3: Create Connection State Tracking

- Add a method `CheckConnectionState()` that:
  - Checks `NetworkManager.Singleton.IsConnectedClient`
  - Checks `NetworkManager.Singleton.IsConnectingClient`
  - Checks if scene is fully loaded
  - Returns connection status

### Step 2.4: Show Connecting Indicator

- When connection starts:
  - Show loading spinner
  - Show "Connecting..." text
  - Hide other UI elements (or show minimal UI)
- Trigger this:
  - In `Start()` if not yet connected
  - When Join/Host button is clicked (before connection completes)

### Step 2.5: Hide Connecting Indicator

- When connection succeeds:
  - Hide loading spinner
  - Hide "Connecting..." text
  - Show normal lobby UI
- Trigger this:
  - In `OnNetworkSpawn()` (when successfully connected)
  - Or check connection state in Update() and hide when connected

### Step 2.6: Show Waiting for Players Indicator

- When connected but only 1 player:
  - Show "Waiting for players..." text
  - Optionally show loading animation
  - Hide this when second player joins
- Check player count: `NetworkManager.Singleton.ConnectedClients.Count`

### Step 2.7: Handle Connection Failures

- If connection fails:
  - Show error message
  - Hide loading indicators
  - Show retry button or return to main menu
- Subscribe to `OnClientDisconnectCallback` to detect disconnections

### Step 2.8: Integrate with UI State System

- Update your `UpdateUIForState()` method to:
  - Show loading indicators in `Connecting` state
  - Show waiting text in `WaitingForPlayers` state
  - Hide all indicators in other states

### Step 2.9: Add Smooth Transitions (Optional)

- Use coroutines or animations to:
  - Fade in/out loading indicators
  - Animate spinner rotation
  - Smooth transitions between states

---

## Part 3: Integration and Testing

### Step 3.1: Connect Systems Together

- Ensure UI state system controls loading indicators
- Ensure state changes trigger appropriate indicator visibility
- Test that indicators appear/disappear at correct times

### Step 3.2: Test Scenarios

- **Test as host:**
  - Start → Connecting → WaitingForPlayers → ConfiguringRules → WaitingForReady → AllReady
- **Test as client:**
  - Join → Connecting → WaitingForReady → AllReady
- **Test disconnection:**
  - Show appropriate error/loading states

### Step 3.3: Polish

- Adjust timing (how long to show indicators)
- Add visual feedback (colors, animations)
- Ensure UI feels responsive

---

## Implementation Order

1. **Start with Part 1** (UI State Management) - This is the foundation
2. **Then Part 2** (Loading Indicators) - This builds on the state system
3. **Finally Part 3** (Integration) - This ties everything together

---

## Tips

- **Start simple**: Get basic show/hide working first, then add animations
- **Use Unity's Event System**: Consider using UnityEvents to decouple state changes from UI updates
- **Debug logging**: Add Debug.Log statements to track state changes
- **Test incrementally**: Test each state transition as you implement it

---

## Notes

- All UI state management is **local** - each client manages its own UI
- The underlying data (player count, ready status) comes from the network
- UI reacts to network data but doesn't need to sync UI state itself
- This keeps the system simple and performant
