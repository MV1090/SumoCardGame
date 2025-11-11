using UnityEngine;

public class WrestlingCard : BaseCard
{
    public override void SafeUpdateCardVisuals()
    {
        base.SafeUpdateCardVisuals();

        if (cardData == null)
        {
           
        }
        else
        {
            if (cardData is WrestlingCard_Scriptable wrestlingCardData)
            {
               
            }
            else
            {
               
            }
        }
    }
}
