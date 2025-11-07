using TMPro;
using UnityEditor;
using UnityEngine;

public class SumoCard : MonoBehaviour
{
    public SumoCard_Scriptable sumoCardData;

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
        nameText.text = sumoCardData.cardName;
        descriptionText.text = "Ability: /n" + sumoCardData.cardDescription;
        artworkImage.sprite = sumoCardData.cardSprite;
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

    private void SafeUpdateCardVisuals()
    {
        if (sumoCardData == null)
        {
            if (nameText) nameText.text = "";
            if (descriptionText) descriptionText.text = "";
            if (artworkImage) artworkImage.sprite = null;
        }
        else
        {
            if (nameText) nameText.text = sumoCardData.cardName;
            if (descriptionText) descriptionText.text = "Ability: \n"  + sumoCardData.cardDescription;
            if (artworkImage) artworkImage.sprite = sumoCardData.cardSprite;
        }
    }

}
