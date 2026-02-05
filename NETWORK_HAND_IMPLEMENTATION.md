# Network Hand Implementation - Progress Log

## Goal
Make player hands network-synchronized so:
- Player's hand is at the bottom of the screen, face up
- Opponent's hand is at the top of the screen, face down
- Both players see both hands (but opponent's cards are face down)

## Approach: Option A - NetworkObject Cards
- Make card prefabs NetworkObjects
- Server spawns cards and assigns ownership
- Each client sees all cards but renders them based on ownership

## Implementation Steps

### ✅ Step 1: Make Card Prefabs NetworkObjects
- [ ] Add NetworkObject component to card prefabs
- [ ] Add card prefabs to NetworkManager's Network Prefabs list

### 🔄 Step 2: Convert BaseCard to NetworkBehaviour (IN PROGRESS)
**Current Status:**
- ✅ BaseCard is already NetworkBehaviour
- ✅ Has `CardOwnerId` NetworkVariable
- ⏳ Need to add:
  - [ ] `cardTypeId` NetworkVariable (to sync which card this is)
  - [ ] CardCatalog reference (to look up card data from type ID)
  - [ ] `OnNetworkSpawn()` logic to initialize card data from type ID
  - [ ] Ownership check methods (`IsOwnedByLocalPlayer()`, `IsOwnedByOpponent()`)
  - [ ] Face-up/face-down visibility method (placeholder for now)

**Code Structure Needed:**
```csharp
public NetworkVariable<int> cardTypeId = new NetworkVariable<int>(-1, 
    NetworkVariableReadPermission.Everyone, 
    NetworkVariableWritePermission.Server);

// CardCatalog reference (static or find in scene)
private static CardCatalog_Scriptable cardCatalog;

public override void OnNetworkSpawn()
{
    base.OnNetworkSpawn();
    // Look up cardData from cardTypeId if needed
    // Call SafeUpdateCardVisuals()
}

public bool IsOwnedByLocalPlayer()
{
    // Check CardOwnerId.Value == NetworkManager.Singleton.LocalClientId
}

public bool IsOwnedByOpponent()
{
    // Check CardOwnerId.Value != NetworkManager.Singleton.LocalClientId
}

public void UpdateCardVisibility()
{
    // Placeholder - implement in Step 5
}
```

### ⏳ Step 3: Modify NetworkCardDeck Spawning
**Current Flow (needs to change):**
- Server receives `RequestDrawCardServerRpc`
- Server calls `SpawnDrawnCardClientRpc` (targeted to requester)
- Client spawns locally

**New Flow (to implement):**
- Server receives `RequestDrawCardServerRpc`
- Server spawns card as NetworkObject using `NetworkObject.Spawn()`
- Server assigns ownership to requester
- Server sets `cardTypeId` and `CardOwnerId` on the card
- All clients see the card automatically (via network sync)

**Changes needed in NetworkCardDeck.cs:**
- Remove `SpawnDrawnCardClientRpc` (no longer needed)
- In `RequestDrawCardServerRpc`, spawn card as NetworkObject
- Use `NetworkObject.SpawnWithOwnership(requester)` or set ownership after spawn
- Set card's `cardTypeId` and `CardOwnerId` NetworkVariables
- Card will automatically sync to all clients

### ⏳ Step 4: Update HandManager to Track NetworkObjects
**Changes needed:**
- Change from `List<GameObject>` to track NetworkObjects
- Add logic to determine card ownership (check `CardOwnerId`)
- Update `AddCardToHand()` to handle both local and opponent cards
- Separate lists or logic for `playerHandCards` vs `opponentHandCards`

**Key Methods:**
```csharp
public void AddCardToHand(NetworkObject cardNetworkObject)
{
    BaseCard card = cardNetworkObject.GetComponent<BaseCard>();
    
    if (card.IsOwnedByLocalPlayer())
    {
        // Add to playerHandCards
        // Position at playerHandAnchor (bottom)
    }
    else
    {
        // Add to opponentHandCards
        // Position at opponentHandTransform (top)
    }
    
    // Update card visibility
    card.UpdateCardVisibility();
}
```

### ⏳ Step 5: Add Face-Up/Face-Down Rendering
**Implementation options:**
- Option A: Hide front renderers, show back sprite (recommended)
- Option B: Swap materials/sprites
- Option C: Rotate card 180 degrees

**In BaseCard:**
- Add card back sprite/material reference
- In `UpdateCardVisibility()`:
  - If owned by local player: show front, hide back
  - If owned by opponent: hide front, show back

### ⏳ Step 6: Position Cards Based on Ownership
**In HandManager:**
- Update `RepositionHand()` to check ownership
- Position owner cards at `playerHandAnchor` (bottom)
- Position opponent cards at `opponentHandTransform` (top)
- May need separate reposition methods for each hand

## Current Files Being Modified

1. **BaseCard.cs** - Converting to full NetworkBehaviour with card data sync
2. **NetworkCardDeck.cs** - Changing from ClientRPC spawning to NetworkObject spawning
3. **HandManager.cs** - Updating to track NetworkObjects and handle ownership

## Key Design Decisions Made

1. **Card Data Sync:** Store card type ID, look up card data from CardCatalog on each client
2. **Ownership:** Use NetworkObject ownership + CardOwnerId NetworkVariable
3. **Visibility:** Check ownership to determine face-up/face-down
4. **Positioning:** Separate anchors for player (bottom) and opponent (top) hands

## Next Steps

1. Complete Step 2: Add cardTypeId NetworkVariable and initialization logic to BaseCard
2. Add ownership check methods to BaseCard
3. Move to Step 3: Modify NetworkCardDeck to spawn NetworkObjects

## Notes

- Card prefabs need NetworkObject component added
- Card prefabs need to be in NetworkManager's Network Prefabs list
- CardCatalog reference needed (can be static or found in scene)
- Need card back sprite/material for face-down rendering

