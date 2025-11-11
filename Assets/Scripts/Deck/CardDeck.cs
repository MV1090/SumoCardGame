using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{ 
    public Deck_Scriptable deck;
    public GameObject wrestlingCardPrefab;
    public GameObject utilityCardPrefab;

    private Queue<Card_Scriptable> deckQueue;

    private void Start()
    {
        BuildDeck();
    }

    private void BuildDeck()
    {
        List<Card_Scriptable> shuffledDeck = deck.BuildShuffledDeck();
        deckQueue = new Queue<Card_Scriptable>(shuffledDeck);
    }

    public GameObject DrawCard(Vector3 newPosition)
    {
        if (deckQueue.Count == 0)
        {
            Debug.LogWarning("Deck is empty!");
            return null;
        }

        Card_Scriptable drawnCard = deckQueue.Dequeue();
        GameObject cardObject = null;

        if (drawnCard is WrestlingCard_Scriptable)
        {
            cardObject = Instantiate(wrestlingCardPrefab, newPosition, Quaternion.identity);
        }
        else if (drawnCard is UtilityCard_Scriptable)
        {
            cardObject = Instantiate(utilityCardPrefab, newPosition, Quaternion.identity);
        }

        if (cardObject != null)
        {
            BaseCard cardDisplay = cardObject.GetComponent<BaseCard>();
            if (cardDisplay != null)
            {
                cardDisplay.cardData = drawnCard;
                cardDisplay.SafeUpdateCardVisuals();
            }
        }

        return cardObject;
    }
}
