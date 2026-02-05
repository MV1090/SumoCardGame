using UnityEngine;
using UnityEngine.UI;

public class DrawCardButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake()
    {        
        button.interactable = false;
    }

    private void Start()
    {        
        TryRegister();
    }

    private void Update()
    {        
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
