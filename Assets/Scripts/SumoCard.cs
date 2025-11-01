using UnityEngine;

[CreateAssetMenu(fileName = "SumoCard", menuName = "Cards/SumoCard")]
public class SumoCard : Card
{
    public int sumoStamina;
    
    public Ability sumoAbility;

    private void OnValidate()
    {
        if (sumoAbility == null) { 
            cardDescription = "";
            return;
        }
        
        cardDescription = sumoAbility.abilityDescription;
        sumoAbility.ActivateAbility();
    }
}
