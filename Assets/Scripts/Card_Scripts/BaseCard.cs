using TMPro;
using UnityEditor;
using UnityEngine;

public class BaseCard : MonoBehaviour
{
public Card_Scriptable cardData;

    [Header("UI References")]
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    
    public SpriteRenderer artworkImage;

    private void Awake()
    {
        //InitCard();
    }

    private void InitCard()
    {
        nameText.text = cardData.cardName;
        descriptionText.text = "Ability: /n" + cardData.cardDescription;
        artworkImage.sprite = cardData.cardSprite;
    }

    // for editor updates   
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorApplication.delayCall += () =>
            {
                if (this == null) return; // object might have been destroyed
                SafeUpdateCardVisuals();
            };
            return;
        }
#endif
        SafeUpdateCardVisuals();
    }

    public virtual void SafeUpdateCardVisuals()
    {
        if (cardData == null)
        {
            if (nameText) nameText.text = "";
            if (descriptionText) descriptionText.text = "";
            if (artworkImage) artworkImage.sprite = null;
        }
        else
        {
            if (nameText) nameText.text = cardData.cardName;
            if (descriptionText) descriptionText.text = "Ability: \n"  + cardData.cardDescription;
            if (artworkImage) artworkImage.sprite = cardData.cardSprite;
        }
    }

}
