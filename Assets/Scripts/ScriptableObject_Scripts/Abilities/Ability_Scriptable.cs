using UnityEngine;

public class Ability_Scriptable : ScriptableObject
{
    public string abilityName;
    [TextArea] public string abilityDescription; 
    
    public bool isPassive;

    public bool isActive;

    public bool isInstant;

    public virtual void ActivateAbility()
    {
        
    }

    public virtual void RemoveAbility()
    {
        
    }

}
