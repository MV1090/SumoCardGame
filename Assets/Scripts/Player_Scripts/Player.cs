using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    // Static reference to the local player instance (not all players)
    public static Player localInstance;
    public static int playerID;
    
    [Header("Player Components")]
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerConnectionHandler connectionHandler;

    [Header("Player Data")]
    [SerializeField] private NetworkPlayerData playerData;
    [SerializeField] private SumoCard_Scriptable currentSumoCard;
    [SerializeField] private NetworkCardDeck activeDeck;

    public PlayerStats Stats => stats;

    private void Awake()
    {
               
        // Get or add PlayerConnectionHandler component
        if (connectionHandler == null)
        {
            connectionHandler = GetComponent<PlayerConnectionHandler>();
            if (connectionHandler == null)
            {
                connectionHandler = gameObject.AddComponent<PlayerConnectionHandler>();                
            }
        }              
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        // Set local instance only if this is the local player
        if (IsOwner)
        {
            if (localInstance == null)
            {
                localInstance = this;
            }
            else if (localInstance != this)
            {
                Debug.LogWarning("Multiple local player instances detected! Destroying duplicate.");
                if (IsServer)
                {
                    GetComponent<NetworkObject>().Despawn();
                }
                return;
            }
        }
        
        // Get or add PlayerStats component if not already assigned
        if (stats == null)
        {
            stats = GetComponent<PlayerStats>();
            if (stats == null)
            {
                stats = gameObject.AddComponent<PlayerStats>();
            }
        }       
        
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        
        // Clear local instance if this was the local player
        if (IsOwner && localInstance == this)
        {
            localInstance = null;
        }
    }

    /// <summary>
    /// Called by PlayerConnectionHandler when player is disconnecting.
    /// Cleans up player-specific resources.
    /// </summary>
    public void OnPlayerDisconnecting()
    {
        // Clean up deck
        if (activeDeck != null)
        {
            // Destroy deck if needed
            activeDeck = null;
        }

        // Clear current Sumo card
        currentSumoCard = null;

        // Reset stats if needed
        if (stats != null)
        {
            stats.ResetStats();
        }
    }

    /// <summary>
    /// Gets the PlayerStats component
    /// </summary>
    public PlayerStats GetStats()
    {
        return stats;
    }

    /// <summary>
    /// Gets the PlayerConnectionHandler component
    /// </summary>
    public PlayerConnectionHandler GetConnectionHandler()
    {
        return connectionHandler;
    }   
    
    /// <summary>
    /// Initializes player stats from a SumoCard. Should be called when a Sumo is selected.
    /// </summary>
    public void InitializeFromSumoCard(SumoCard_Scriptable sumoCard)
    {
        if (!IsServer) return;

        currentSumoCard = sumoCard;
        
        if (stats != null)
        {
            stats.InitializeFromSumoCard(sumoCard);
        }
        
        Debug.Log($"Player stats initialized from SumoCard: {sumoCard.cardName}");
    }

    /// <summary>
    /// Gets the current SumoCard for this player
    /// </summary>
    public SumoCard_Scriptable GetCurrentSumoCard()
    {
        return currentSumoCard;
    }

    /// <summary>
    /// Gets the NetworkPlayerData for this player
    /// </summary>
    public NetworkPlayerData GetPlayerData()
    {
        return playerData;
    }

    /// <summary>
    /// Sets the NetworkPlayerData for this player
    /// </summary>
    public void SetPlayerData(NetworkPlayerData data)
    {
        playerData = data;
        if (playerData != null && IsServer)
        {
            playerData.SetClientId(OwnerClientId);
        }
    } 
    public void InitializeDeck(NetworkCardDeck deck)
    {
        activeDeck = deck;
        Debug.Log($"Deck initialized for player");
    }

    public bool IsDeckInitialized()
    {
        return activeDeck != null;
    }

    public void DrawCard()
    {
        if (!IsOwner)
            return;

        if (activeDeck == null)
        {
            Debug.LogWarning("Cannot draw card: Deck has not been initialized yet.");
            return;
        }

        if (HandManager.Instance == null)
        {
            Debug.LogWarning("Cannot draw card: HandManager is not available.");
            return;
        }

        if (HandManager.Instance.IsHandFull())
        {
            Debug.Log("Hand is full, cannot draw more cards.");
            return;
        }

        activeDeck.RequestDrawCardServerRpc();        
    }
}

//    [ServerRpc]
//    private void DrawCardServerRpc()
//    {
//        if (activeDeck == null)
//        {
//            Debug.LogWarning("Cannot draw card: Deck has not been initialized yet.");
//            return;
//        }

//        if (HandManager.Instance == null)
//        {
//            Debug.LogWarning("Cannot draw card: HandManager is not available.");
//            return;
//        }

//        if (HandManager.Instance.IsHandFull())
//        {
//            Debug.Log("Hand is full, cannot draw more cards.");
//            return;
//        }

//        GameObject drawnCard = activeDeck.RequestDrawCardServerRpc();

//        if (drawnCard != null)
//            HandManager.Instance.AddCardToHand(drawnCard);
//    }
//}
