using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/ New Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    [TextArea] public string abilityDescription;

    public int abilityBonus;

    public AbilityType abilityType;

    public enum AbilityType
    {
        StrengthBonus,
        DefenseBonus,
        DualBonus,
        HealBonus
    }

    public void ActivateAbility()
    {
        switch(abilityType)
        {
            case AbilityType.StrengthBonus:
                //applies strength bonus
                break;
            case AbilityType.DefenseBonus:
                //applies defense bonus
                break;
            case AbilityType.DualBonus:
                //applies both bonuses
                break;
            case AbilityType.HealBonus:
                //applies heal bonus
                break;
        }
    }

    public void RemoveAbility()
    {
        switch (abilityType)
        {
            case AbilityType.StrengthBonus:
                //removes strength bonus
                break;
            case AbilityType.DefenseBonus:
                //removes defense bonus
                break;
            case AbilityType.DualBonus:
                //removes both bonuses
                break;
            case AbilityType.HealBonus:
                //removes heal bonus
                break;
        }
    }
}
