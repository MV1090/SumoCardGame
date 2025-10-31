using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/New Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    [TextArea]public string abilityDescription;
    public int bonus;

    public AbilityType abilityType;    

    public enum AbilityType
    {
        Attack,
        Defence,
        Stamina
    }

    public virtual void ApplyEffect(SumoCard card)
    {
        // switch(abilityType)
        // {
        //     case AbilityType.Attack:
        //         card.attack += bonus;
        //         break;
        //     case AbilityType.Defence:
        //         card.defence += bonus;
        //         break;
        //     case AbilityType.Stamina:
        //         card.sumoStamina += bonus;
        //         break;
        // }
    }
    
    public virtual void RemoveEffect(SumoCard card)
    {   
        // switch(abilityType)
        // {
        //     case AbilityType.Attack:
        //         card.attack -= bonus;
        //         break;
        //     case AbilityType.Defence:
        //         card.defence -= bonus;
        //         break;
        //     case AbilityType.Stamina:
        //         card.sumoStamina -= bonus;
        //         break;
        // }
    }
}
