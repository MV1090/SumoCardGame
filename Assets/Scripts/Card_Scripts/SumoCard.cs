using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SumoCard : BaseCard
{
    public TMP_Text staminaText;

    public override void SafeUpdateCardVisuals()
    {
        base.SafeUpdateCardVisuals();
        if (cardData == null) { 
            if (staminaText) 
                staminaText.text = ""; 
        }
        else 
        {
            if (cardData is SumoCard_Scriptable sumoCardData)
            {
                if (staminaText)
                    staminaText.text = sumoCardData.sumoStamina.ToString();
            }
            else 
            {
                if (staminaText)
                    staminaText.text = "";
            }
        }
    }

}
