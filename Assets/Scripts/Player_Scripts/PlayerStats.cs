using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    [SerializeField] private int baseStamina;
    public int currentStamina;
    [SerializeField] private int strength;
    public int currentStrength;
    [SerializeField] private int defense;
    public int currentDefense;
    [SerializeField] private int weight;
    public int currentWeight;
    [SerializeField] private int handSize;
    public int currentHandSize;
    [SerializeField] private int cardsToDraw;
    public int currentCardsToDraw;

    public void InitializeStats()
    {
        currentStamina = baseStamina;
        currentStrength = strength;
        currentDefense = defense;
        currentWeight = weight;
        currentHandSize = handSize;
        currentCardsToDraw = cardsToDraw;
    }

    public void ResetStats()
    {
        currentStamina = baseStamina;
        currentStrength = strength;
        currentDefense = defense;
        currentWeight = weight;
        currentHandSize = handSize;
        currentCardsToDraw = cardsToDraw;
    }
}
