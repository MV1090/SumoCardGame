using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PlayerConnectionHandler : NetworkBehaviour
{
    public NetworkVariable<int> playerId = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Start()
    {
        // Listen for disconnects globally
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Host always has ClientId 0 in Netcode for GameObjects
            playerId.Value = OwnerClientId == 0 ? 1 : 2;
        }

        Debug.Log($"Player spawned. Owner={OwnerClientId}, ID={playerId.Value}");
    }


    private void OnClientDisconnected(ulong clientId)
    {
        // If we are the one who disconnected, ignore
        if (clientId == OwnerClientId)
            return;

        // At this point, opponent has disconnected
        Debug.LogWarning("Opponent disconnected!");

        // Only the remaining player executes this
        if (IsOwner)
        {
            // If we are in the GameScene, return to MainMenu
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                Debug.Log("Returning to MainMenu due to disconnect...");
                SceneManager.LoadScene("MainMenuScene");
            }
        }
    }
}

