using UnityEngine;

public class Card : ScriptableObject
{
    public string cardName;  
    [TextArea] public string cardDescription;
    public Sprite cardSprite;
}
