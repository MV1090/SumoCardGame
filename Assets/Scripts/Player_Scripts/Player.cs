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
    }
    public PlayerStats stats;

    [SerializeField] private SumoCard_Scriptable currentSumoCard;
    [SerializeField] CardDeck activeDeck; 

    public void DrawCard()
    {
        activeDeck.DrawCard(Vector3.zero); 
    }
}
