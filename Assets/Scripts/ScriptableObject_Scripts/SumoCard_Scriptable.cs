using UnityEngine;

[CreateAssetMenu(fileName = "SumoCard", menuName = "Cards/Sumo Card")]
public class SumoCard_Scriptable : Card_Scriptable
{
    public int sumoStamina;
    public Ability_Scriptable sumoAbility;


    //for editor updates
    private void OnValidate()
    {
        if (sumoAbility == null)
        {
            cardDescription = "";
            return;
        }

        cardDescription = sumoAbility.abilityDescription;
        sumoAbility.ActivateAbility();
    }
}
