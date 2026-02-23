using UnityEngine;

public class Card_Scriptable : ScriptableObject
{
    public string cardName;  
    [TextArea] public string cardDescription;

    public Sprite cardArtWork;
    public Sprite cardFaceSprite;
    public Sprite cardBackSprite;
}
