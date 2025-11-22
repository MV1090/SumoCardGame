using UnityEngine;

/// <summary>
/// ScriptableObject containing player identification and configuration data.
/// This is used for player information that persists across the network session
/// and is set before or when a player joins a game.
/// 
/// Note: Runtime stats (stamina, strength, defense, etc.) should be NetworkVariables
/// in the Player NetworkBehaviour, not stored in this ScriptableObject.
/// </summary>
[CreateAssetMenu(fileName = "NetworkPlayerData", menuName = "Player/Network Player Data")]
public class NetworkPlayerData : ScriptableObject
{
    [Header("Player Identification")]
    [Tooltip("The display name for this player")]
    public string playerName = "Player";
    
    [Header("Visual Identity")]
    [Tooltip("Color used to identify this player in the UI")]
    public Color playerColor = Color.white;
    
    [Tooltip("Optional icon/avatar for this player")]
    public Sprite playerIcon;
    
    [Header("Network Information")]
    [Tooltip("The network client ID (set at runtime, not in editor)")]
    [SerializeField, ReadOnly]
    private ulong clientId;
    
    /// <summary>
    /// Gets the client ID for this player (set at runtime when player connects)
    /// </summary>
    public ulong ClientId => clientId;
    
    /// <summary>
    /// Sets the client ID for this player. Should be called when the player connects.
    /// </summary>
    public void SetClientId(ulong id)
    {
        clientId = id;
    }
    
    /// <summary>
    /// Resets the player data to default values (useful for reconnection scenarios)
    /// </summary>
    public void Reset()
    {
        clientId = 0;
    }
}

