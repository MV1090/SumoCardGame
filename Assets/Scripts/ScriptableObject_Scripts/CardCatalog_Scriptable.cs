using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardCatalog_Scriptable", menuName = "Cards/CardCatalog_Scriptable")]
public class CardCatalog_Scriptable : ScriptableObject
{
    public List<Card_Scriptable> allCards = new List<Card_Scriptable>();

    public List<Card_Scriptable> GetAllCards()
    {
        return allCards;
    }

    public Card_Scriptable GetCard(int ID)
    {
        if (ID < 0 || ID >= allCards.Count)
        {
            return null;
        }

        return allCards[ID];
    }

    public int GetCardID(Card_Scriptable card)
    {
        return allCards.IndexOf(card);
    }

}
