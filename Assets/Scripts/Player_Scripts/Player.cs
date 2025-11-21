using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    private void Awake()
    {
        if(instance == null){
            instance = this;
        } else
        {
            Destroy(gameObject);
        }

        stats.InitializeStats();
    }
    public PlayerStats stats;

    [SerializeField] private SumoCard_Scriptable currentSumoCard;
    [SerializeField] private CardDeck activeDeck; // Can be null until game starts

    /// <summary>
    /// Initializes the deck for this player. Should be called when the game starts,
    /// after Sumo selection is complete.
    /// </summary>
    /// <param name="deck">The CardDeck to assign to this player</param>
    public void InitializeDeck(CardDeck deck)
    {
        activeDeck = deck;
        Debug.Log($"Deck initialized for player");
    }

    /// <summary>
    /// Checks if the player's deck is initialized and ready
    /// </summary>
    public bool IsDeckInitialized()
    {
        return activeDeck != null;
    }

    public void DrawCard()
    {
        // Check if deck is initialized
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

        GameObject drawnCard = activeDeck.DrawCard(); 

        if(drawnCard != null)
            HandManager.Instance.AddCardToHand(drawnCard);
    }
}
