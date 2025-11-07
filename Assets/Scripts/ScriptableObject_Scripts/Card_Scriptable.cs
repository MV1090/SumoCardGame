using UnityEngine;

public class Card_Scriptable : ScriptableObject
{
    public string cardName;  
    [TextArea] public string cardDescription;
    public Sprite cardSprite;
}
