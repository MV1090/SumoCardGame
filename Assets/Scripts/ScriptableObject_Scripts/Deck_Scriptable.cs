using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "New Deck")]
public class Deck_Scriptable : ScriptableObject
{
    [System.Serializable]
    public class WrestlingCardEntry
    {
        public WrestlingCard_Scriptable card;
        [Range(1,5)]
        public int quantity;
    }

    [System.Serializable]
    public class UtilityCardEntry
    {
        public UtilityCard_Scriptable card;
        [Range(1, 5)]
        public int quantity;
    }

    public List<WrestlingCardEntry> wrestlingCards;
    public List<UtilityCardEntry> utilityCards;

    public int GetTotalCardCount()
    {
        int total = 0;

        foreach (var entry in wrestlingCards)
        {
            total += entry.quantity;
        }

        foreach (var entry in utilityCards)
        {
            total += entry.quantity;
        }

        return total;
    }

    public List<Card_Scriptable> BuildShuffledDeck()
    {
        List<Card_Scriptable> deck = new List<Card_Scriptable>();

        foreach (var entry in wrestlingCards)
        {
            for (int i = 0; i < entry.quantity; i++)
            {
                deck.Add(entry.card);
            }
        }

        foreach (var entry in utilityCards)
        {
            for (int i = 0; i < entry.quantity; i++)
            {
                deck.Add(entry.card);
            }
        }

        // Shuffle the deck
        for (int i = 0; i < deck.Count; i++)
        {
            Card_Scriptable temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }

        return deck;
    }

}
