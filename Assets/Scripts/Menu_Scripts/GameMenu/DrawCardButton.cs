using UnityEngine;
using UnityEngine.UI;

public class DrawCardButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake()
    {        
        button.interactable = false; // Disabled until local player exists
    }

    private void Start()
    {
        // Try to register once at start
        TryRegister();
    }

    private void Update()
    {
        // If still not registered, keep trying until player spawns
        if (Player.localInstance != null && button.onClick.GetPersistentEventCount() == 0)
        {
            TryRegister();
        }
    }

    private void TryRegister()
    {
        if (Player.localInstance == null)
            return;

        Debug.Log("Registered DrawCard button to local player.");

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            Player.localInstance.DrawCard();
        });

        button.interactable = true;
    }
}
