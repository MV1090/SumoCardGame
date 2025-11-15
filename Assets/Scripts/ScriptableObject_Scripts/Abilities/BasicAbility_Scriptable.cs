using UnityEngine;

[CreateAssetMenu(fileName = "BasicAbility", menuName = "Abilities/Basic Ability")]
public class BasicAbility_Scriptable : Ability_Scriptable
{
    public int abilityBonus;

    public AbilityType abilityType;
    public enum AbilityType
    {
        StrengthBonus,
        DefenseBonus,
        HealBonus,
        HandSizeBonus,        
        IncreaseWeightBonus,
        ReduceStrength,
        ReduceDefense,
        ReduceOpponentsWeight,
        ReduceOpponentsHandSize,
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();

        switch (abilityType)
        {
            case AbilityType.StrengthBonus:
                //applies strength bonus
                break;
            case AbilityType.DefenseBonus:
                //applies defense bonus
                break;
            case AbilityType.HealBonus:
                //applies heal bonus
                break;
        }
    }

    public override void RemoveAbility()
    {
        base.RemoveAbility();

        switch (abilityType)
        {
            case AbilityType.StrengthBonus:
                //removes strength bonus
                break;
            case AbilityType.DefenseBonus:
                //removes defense bonus
                break;
            case AbilityType.HealBonus:
                //removes heal bonus
                break;
        }
    }
}
