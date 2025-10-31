using UnityEngine;
using UnityEngine.UI;   
using System;

[CreateAssetMenu(fileName = "SumoCard", menuName = "Card Objects/New Sumo")]
public class SumoCard : Card
{
    public int sumoStamina;

    [SerializeField] private Ability ability;

    //public event Action<Ability> OnAbilityChanged;

    // public Ability Ability
    //     {
    //         get => ability;
    //         set => SetAbility(value);
    //     }

    private Ability previousAbility;

    private void OnEnable()
    {
        previousAbility = ability;
    }

    private void OnValidate()
    {
        if (previousAbility != ability)
        {
            //Ability old = previousAbility;
            previousAbility = ability;

            if(ability == null)
            cardDescription = "";
        else
            cardDescription = ability.abilityDescription;
            //OnAbilityChanged?.Invoke(ability);
        }
    }

    public void SetAbility(Ability newAbility)
    {
        // if (ability == newAbility) return;

        // Ability old = ability;
        // previousAbility = newAbility;
        // ability = newAbility;

        // if(ability == null)
        //     cardDescription = "";
        // else
        // cardDescription = ability.abilityDescription;
        // OnAbilityChanged?.Invoke(ability);
    }
}
   
    
