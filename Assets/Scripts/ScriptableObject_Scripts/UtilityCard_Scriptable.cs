using UnityEngine;

[CreateAssetMenu(fileName = "UtilityCard", menuName = "Cards/Utility Card")]
public class UtilityCard_Scriptable : Card_Scriptable
{
    public Ability utilityAbility;

    private void OnValidate()
    {
        if (utilityAbility == null)
        {
            cardDescription = "";
            return;
        }

        cardDescription = utilityAbility.abilityDescription;       
    }

    public void PlayCard()
    {
        utilityAbility.ActivateAbility();
    }
    public void RemoveCard()
    {
        utilityAbility.RemoveAbility();
    }
}
