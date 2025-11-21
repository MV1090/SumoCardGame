using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : NetworkBehaviour
{
    [SerializeField] Button startGameButton;
    [SerializeField] TMP_Text playerReady;


    public static Dictionary<ulong, bool> ReadyStates = new Dictionary<ulong, bool>();

    public NetworkObject networkObject;


    public override void OnNetworkSpawn()
    {
        Debug.Log($"OnNetworkSpawn: {IsSpawned}, Client ID: {NetworkManager.LocalClientId}");
        
    }

    void Start()
    {
        startGameButton.onClick.AddListener(OnStartGameClicked);
        UpdatePlayerReadyStatus(false);
        Debug.Log($"IsSpawned: {networkObject.IsSpawned}");
    }

    void OnStartGameClicked()
    {
        Debug.Log("Start Game button clicked!");
        UpdatePlayerReadyStatus(true);
                       
        SetPlayerReadyServerRpc(NetworkManager.Singleton.LocalClientId, true);     
    }

    public void UpdatePlayerReadyStatus(bool isReady)
    {
        playerReady.text = isReady ? "Ready" : "Not Ready";
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerReadyServerRpc(ulong playerId, bool isReady)
    {
        if (ReadyStates.ContainsKey(playerId))
        {
            ReadyStates[playerId] = isReady;
        }
        else
        {
            ReadyStates.Add(playerId, isReady);
        }

        Debug.Log($"Player {playerId} ready state updated to: {isReady}");

        CheckAllReady();
    }

    private void CheckAllReady()
    {
        if(ReadyStates.Count < 2)
        {
            Debug.Log("Not enough players to start the game");
            return; 
        }

        foreach (var state in ReadyStates.Values)
        {
            if (!state)
            {
                return; // At least one player is not ready
            }
        }

        Debug.Log("All players are ready! Starting the game...");

        NetworkSceneManager.Instance.LoadGameScene();
    }
}
