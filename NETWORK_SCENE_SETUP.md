# Network Scene Management Setup Guide

## What is Network Scene Management?

Network Scene Management allows the host/server to load scenes, and all connected clients will automatically load the same scene. This is essential for multiplayer games where you need to transition between different scenes.

## Important: Scene Flow Understanding

**Main Menu is NOT a networked scene** - it's loaded locally before networking starts.

### Game Flow:
1. **Main Menu** (Local Scene) - No networking, each player loads independently
   - Player clicks "Host Game" → `StartHost()` is called
   - Player clicks "Join Game" → `StartClient()` is called
   - Still in Main Menu, but now networked

2. **Lobby Scene** (First Networked Scene) - Host loads, clients follow
   - Host calls `NetworkSceneManager.LoadLobbyScene()`
   - All connected clients automatically load Lobby

3. **Game Scene** (Second Networked Scene) - Host loads, clients follow
   - Host calls `NetworkSceneManager.LoadGameScene()`
   - All connected clients automatically load Game Scene

4. **Return to Main Menu** - Disconnect and load locally
   - Call `NetworkSceneManager.ReturnToMainMenu()`
   - Disconnects from network and loads Main Menu locally

## What Was Created

1. **NetworkSceneManager.cs** - A script that handles scene loading and synchronization across the network.

## Setup Steps

### Step 1: Add NetworkSceneManager to Your Scene

1. In Unity, open your scene (currently `SampleScene.unity`)
2. Create an empty GameObject (right-click in Hierarchy → Create Empty)
3. Name it "NetworkSceneManager"
4. Add the `NetworkSceneManager` component to it
5. Make sure this GameObject persists across scenes (optional but recommended):
   - Add a `DontDestroyOnLoad` script or make it a child of a persistent object

### Step 2: Add Scenes to Build Settings

1. Go to **File → Build Settings**
2. Click **Add Open Scenes** to add your current scene
3. For future scenes (Lobby, Game Scene, etc.), create them and add them to the build settings
4. **Important**: The scene names in `NetworkSceneManager.cs` must match your scene file names (without the `.unity` extension)

### Step 3: Verify NetworkManager Settings

1. Select the **NetworkManager** GameObject in your scene
2. In the Inspector, verify that:
   - **Enable Scene Management** is checked ✅ (should already be enabled)
   - **Network Prefabs List** is assigned ✅ (should already be set)

### Step 4: Update Scene Names (When You Create New Scenes)

When you create new scenes, update the constants in `NetworkSceneManager.cs`:

```csharp
// Networked scenes (loaded by host, synced to clients)
public const string LOBBY_SCENE = "LobbyScene";      // Update when you create the lobby
public const string GAME_SCENE = "GameScene";        // Update when you create the game scene

// Local scene (loaded independently, not networked)
public const string MAIN_MENU_SCENE = "MainMenuScene"; // Update when you create the main menu
```

**Important**: 
- Scene names must match your scene file names exactly (without `.unity` extension)
- Main Menu is loaded locally, not through network scene management

## How to Use

### Loading Networked Scenes (Server/Host Only)

**Important**: `NetworkSceneManager.Instance` only exists in networked scenes (Lobby, Game Scene). 

#### Option 1: From Main Menu (use NetworkManager directly)

```csharp
// From Main Menu - NetworkSceneManager doesn't exist yet
if (NetworkManager.Singleton.IsServer)
{
    NetworkManager.Singleton.SceneManager.LoadScene(
        NetworkSceneManager.LOBBY_SCENE, 
        LoadSceneMode.Single
    );
}
```

#### Option 2: From Networked Scenes (use NetworkSceneManager wrapper)

```csharp
// From Lobby/Game Scene - NetworkSceneManager exists
if (NetworkManager.Singleton.IsServer)
{
    NetworkSceneManager.Instance?.LoadGameScene();  // Uses wrapper
    // OR
    NetworkManager.Singleton.SceneManager.LoadScene(
        NetworkSceneManager.GAME_SCENE, 
        LoadSceneMode.Single
    );  // Direct approach
}
```

### Example 1: Loading Lobby After Host Starts (From Main Menu)

**Important**: From Main Menu, `NetworkSceneManager` doesn't exist yet, so use `NetworkManager.SceneManager` directly:

```csharp
// In your Main Menu script, after StartHost() is called
public void OnHostGame()
{
    NetworkManager.Singleton.StartHost();
    
    // Wait a moment for connection, then load lobby
    // (You might want to wait for client to connect first)
    StartCoroutine(LoadLobbyAfterConnection());
}

private IEnumerator LoadLobbyAfterConnection()
{
    yield return new WaitForSeconds(0.5f); // Brief delay
    
    if (NetworkManager.Singleton.IsServer)
    {
        // Use NetworkManager.SceneManager directly from Main Menu
        // NetworkSceneManager.Instance doesn't exist in Main Menu
        NetworkManager.Singleton.SceneManager.LoadScene(
            NetworkSceneManager.LOBBY_SCENE, 
            LoadSceneMode.Single
        );
    }
}
```

**Alternative**: You could also load Lobby locally first (non-networked), then transition to networked mode, but the above approach is simpler.

### Example 2: Loading Game Scene When Both Players Are Ready (From Lobby)

**Note**: From Lobby scene (or any networked scene), you can use `NetworkSceneManager.Instance`:

```csharp
// In your Lobby script
public void OnBothPlayersReady()
{
    // Only the server/host should load scenes
    if (NetworkManager.Singleton.IsServer)
    {
        // NetworkSceneManager exists in Lobby scene, so you can use it
        NetworkSceneManager.Instance?.LoadGameScene();
    }
}
```

**Or from any networked scene, use it directly:**

```csharp
// Alternative: Use NetworkManager.SceneManager directly (works from anywhere)
public void OnBothPlayersReady()
{
    if (NetworkManager.Singleton.IsServer)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(
            NetworkSceneManager.GAME_SCENE, 
            LoadSceneMode.Single
        );
    }
}
```

### Example 3: Returning to Main Menu (Disconnect)

```csharp
public void OnLeaveGame()
{
    // This disconnects and loads Main Menu locally
    NetworkSceneManager.Instance?.ReturnToMainMenu();
}
```

### Important Notes About Main Menu

**Main Menu is NOT loaded through NetworkSceneManager** - it's a local scene:
- Load it normally: `SceneManager.LoadScene("MainMenuScene")`
- Or use: `NetworkSceneManager.ReturnToMainMenu()` (which disconnects first)
- Main Menu exists before networking starts
- Each player loads Main Menu independently

## Important Notes

- **Only the server/host can load networked scenes** - Clients will automatically follow
- **Main Menu is NOT networked** - It's loaded locally before networking starts
- **NetworkSceneManager only exists in networked scenes** - Use `NetworkManager.SceneManager` directly from Main Menu
- **Scene names must match exactly** - Case-sensitive, no `.unity` extension
- **All scenes must be in Build Settings** - Otherwise they won't load
- **NetworkManager must have Scene Management enabled** - This is already configured

## Key Difference: Main Menu vs Networked Scenes

### From Main Menu:
- `NetworkSceneManager.Instance` = **null** (doesn't exist yet)
- Use: `NetworkManager.Singleton.SceneManager.LoadScene()` **directly**

### From Networked Scenes (Lobby, Game Scene):
- `NetworkSceneManager.Instance` = **available** (exists in scene)
- Can use: `NetworkSceneManager.Instance.LoadGameScene()` **OR** `NetworkManager.Singleton.SceneManager.LoadScene()` directly

## Next Steps

1. Create your Main Menu scene (local, no networking)
2. Create your Lobby scene (networked)
3. Create your Game scene (networked)
4. Add all scenes to Build Settings
5. Update the scene name constants in `NetworkSceneManager.cs`
6. Add NetworkSceneManager to Lobby and Game scenes (not Main Menu)
7. Test scene transitions in multiplayer

## Scene Setup Checklist

- [ ] Main Menu scene created (local scene, no NetworkSceneManager needed)
- [ ] Lobby scene created (add NetworkSceneManager GameObject)
- [ ] Game scene created (add NetworkSceneManager GameObject)
- [ ] All scenes added to Build Settings
- [ ] Scene name constants updated in NetworkSceneManager.cs
- [ ] Main Menu has buttons to StartHost() and StartClient()
- [ ] Lobby has logic to load Game Scene when ready
- [ ] Game has logic to return to Main Menu when done

## Troubleshooting

**Problem**: Scene doesn't load
- Check that the scene name matches exactly (case-sensitive)
- Verify the scene is in Build Settings
- Check that NetworkManager has Scene Management enabled

**Problem**: Clients don't load the scene
- Ensure NetworkManager is properly configured
- Check network connection status
- Verify NetworkSceneManager is in the scene

**Problem**: "Only the server can load scenes!" warning
- Make sure you're calling `LoadScene()` from the server/host
- Check `NetworkManager.Singleton.IsServer` before loading

