using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections;


public class MainMenu : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button MatchMakingButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(HostGame);
        joinButton.onClick.AddListener(JoinGame);
        MatchMakingButton.onClick.AddListener(MatchMaking);
    }

    private void HostGame()
    {
        NetworkManager.Singleton.StartHost();

        StartCoroutine(LoadLobbyAfterConnection());

        Debug.Log($"Player Started game as Host: {NetworkManager.Singleton.IsHost}");
        //NetworkManager.Singleton.SceneManager.LoadScene(NetworkSceneManager.LOBBY_SCENE, LoadSceneMode.Single);
    }
    
    private void JoinGame()
    {
        NetworkManager.Singleton.StartClient();

        StartCoroutine(LoadLobbyAfterConnection());

        Debug.Log($"Player Joined game as Client: {NetworkManager.Singleton.IsClient}");
        //NetworkManager.Singleton.SceneManager.LoadScene(NetworkSceneManager.LOBBY_SCENE, LoadSceneMode.Single);
    }

    private void MatchMaking()
    {
        //NetworkManager.Singleton.StartMatchMaker();
        //TODO: Implement Match Making
    }

    private IEnumerator LoadLobbyAfterConnection()
    {
        yield return new WaitForSeconds(0.5f);

        NetworkSceneManager.Instance.LoadLobbyScene();              
    }
}
