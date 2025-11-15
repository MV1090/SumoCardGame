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
    [SerializeField] CardDeck activeDeck; 

    public void DrawCard()
    {
        if (HandManager.Instance.IsHandFull())
        {
            Debug.Log("Hand is full, cannot draw more cards.");
            return;
        }

        GameObject drawnCard = activeDeck.DrawCard(); 

        if(drawnCard!=null)
            HandManager.Instance.AddCardToHand(drawnCard);
    }
}
