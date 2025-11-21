using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages scene loading and synchronization across the network.
/// Handles scene transitions for multiplayer gameplay.
/// 
/// IMPORTANT: This only handles NETWORKED scenes (Lobby, Game Scene).
/// Main Menu is a LOCAL scene loaded before networking starts.
/// 
/// Flow:
/// 1. Main Menu (local) → Player clicks Host/Join → StartHost()/StartClient()
/// 2. Host loads Lobby (networked) → All clients follow
/// 3. Host loads Game Scene (networked) → All clients follow
/// 4. Return to Main Menu → Disconnect and load locally
/// </summary>
public class NetworkSceneManager : NetworkBehaviour
{
    public static NetworkSceneManager Instance { get; private set; }

    // Networked scene names - these should match your scene file names (without .unity extension)
    // NOTE: Main Menu is NOT a networked scene - it's loaded locally before networking starts
    public const string LOBBY_SCENE = "LobbyScene";
    public const string GAME_SCENE = "GameScene";
    
    // Main Menu scene name (for local loading only, not networked)
    public const string MAIN_MENU_SCENE = "MainMenuScene";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Subscribe to scene loading events
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
            NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
        }

        NetworkManager.Singleton.SceneManager.OnSynchronizeComplete += OnSynchronizeComplete;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        // Unsubscribe from events
        if (IsServer && NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;
            NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnLoadComplete;
        }

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.SceneManager.OnSynchronizeComplete -= OnSynchronizeComplete;
        }
    }

    /// <summary>
    /// Loads a scene on the server, which will automatically sync to all clients.
    /// Only the server/host should call this method.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load</param>
    public void LoadScene(string sceneName)
    {
        if (!IsServer)
        {
            if(sceneName == LOBBY_SCENE)
            {
                Debug.Log("Client loading Main Menu locally");
                SceneManager.LoadScene(LOBBY_SCENE, LoadSceneMode.Single);
                return;
            }

            Debug.LogWarning("Only the server can load scenes!");
            return;
        }

        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager is null!");
            return;
        }

        Debug.Log($"Server loading scene: {sceneName}");
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    /// <summary>
    /// Loads the lobby scene
    /// </summary>
    public void LoadLobbyScene()
    {
        LoadScene(LOBBY_SCENE);
    }

    /// <summary>
    /// Loads the game scene
    /// </summary>
    public void LoadGameScene()
    {
        LoadScene(GAME_SCENE);
    }

    /// <summary>
    /// Disconnects from the network and returns to the main menu (local scene load).
    /// This should be called when leaving a game/lobby to return to the main menu.
    /// Main Menu is NOT a networked scene - it's loaded locally.
    /// </summary>
    public void ReturnToMainMenu()
    {
        // Disconnect from network first
        if (NetworkManager.Singleton != null)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.Shutdown();
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.Shutdown();
            }
        }

        // Load main menu locally (not networked)
        Debug.Log("Returning to Main Menu (local scene)");
        SceneManager.LoadScene(MAIN_MENU_SCENE, LoadSceneMode.Single);
    }

    /// <summary>
    /// Called when a scene load event is completed on the server
    /// </summary>
    private void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        Debug.Log($"Scene load completed: {sceneName}");
        
        if (clientsTimedOut.Count > 0)
        {
            Debug.LogWarning($"Some clients timed out loading scene {sceneName}: {clientsTimedOut.Count}");
        }

        // You can add custom logic here when scene loading completes
        // For example, initializing game objects, spawning players, etc.
    }

    /// <summary>
    /// Called when a scene load completes for a specific client
    /// </summary>
    private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        Debug.Log($"Client {clientId} finished loading scene: {sceneName}");
    }

    /// <summary>
    /// Called when scene synchronization is complete
    /// </summary>
    private void OnSynchronizeComplete(ulong clientId)
    {
        Debug.Log($"Client {clientId} synchronized scenes");
    }

    /// <summary>
    /// Gets the current active scene name
    /// </summary>
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// Checks if a specific scene is currently loaded
    /// </summary>
    public bool IsSceneLoaded(string sceneName)
    {
        return SceneManager.GetActiveScene().name == sceneName;
    }
}

