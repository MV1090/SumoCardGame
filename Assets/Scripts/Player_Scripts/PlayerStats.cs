using UnityEngine;
using Unity.Netcode;

/// <summary>
/// NetworkBehaviour that manages all player stat information including base stats 
/// and network-synchronized current stats. All stat operations should go through this class.
/// </summary>
public class PlayerStats : NetworkBehaviour
{
    [Header("Base Stats (set from SumoCard)")]
    [SerializeField] private int baseStamina;
    [SerializeField] private int baseStrength;
    [SerializeField] private int baseDefense;
    [SerializeField] private int baseWeight;
    [SerializeField] private int baseHandSize = 5;
    [SerializeField] private int baseCardsToDraw = 2;

    [Header("Network Synchronized Current Stats")]
    // NetworkVariables for stats that change during gameplay and need synchronization
    public NetworkVariable<int> currentStamina = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public NetworkVariable<int> currentStrength = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public NetworkVariable<int> currentDefense = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public NetworkVariable<int> currentWeight = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public NetworkVariable<int> currentHandSize = new NetworkVariable<int>(5,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    public NetworkVariable<int> currentCardsToDraw = new NetworkVariable<int>(2,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    // Getters for base stats
    public int GetBaseStamina() => baseStamina;
    public int GetBaseStrength() => baseStrength;
    public int GetBaseDefense() => baseDefense;
    public int GetBaseWeight() => baseWeight;
    public int GetBaseHandSize() => baseHandSize;
    public int GetBaseCardsToDraw() => baseCardsToDraw;

    // Getters for current stats (read NetworkVariables)
    public int GetCurrentStamina() => currentStamina.Value;
    public int GetCurrentStrength() => currentStrength.Value;
    public int GetCurrentDefense() => currentDefense.Value;
    public int GetCurrentWeight() => currentWeight.Value;
    public int GetCurrentHandSize() => currentHandSize.Value;
    public int GetCurrentCardsToDraw() => currentCardsToDraw.Value;

    //// Setters for base stats
    //public void SetBaseStamina(int value) => baseStamina = value;
    //public void SetBaseStrength(int value) => baseStrength = value;
    //public void SetBaseDefense(int value) => baseDefense = value;
    //public void SetBaseWeight(int value) => baseWeight = value;
    //public void SetBaseHandSize(int value) => baseHandSize = value;
    //public void SetBaseCardsToDraw(int value) => baseCardsToDraw = value;

    /// <summary>
    /// Initializes current stats from base stats. Should be called when Sumo is selected.
    /// </summary>
    public void InitializeStats(SumoCard_Scriptable sumoCard)
    {
        if (!IsServer) return;

        currentStamina.Value = baseStamina;
        currentStrength.Value = baseStrength;
        currentDefense.Value = baseDefense;
        currentWeight.Value = baseWeight;
        currentHandSize.Value = baseHandSize;
        currentCardsToDraw.Value = baseCardsToDraw;

        InitializeFromSumoCard(sumoCard);
    }

    /// <summary>
    /// Resets current stats to base values (server-authoritative)
    /// </summary>
    public void ResetStats()
    {
        if (!IsServer) return;

        currentStamina.Value = baseStamina;
        currentStrength.Value = baseStrength;
        currentDefense.Value = baseDefense;
        currentWeight.Value = baseWeight;
        currentHandSize.Value = baseHandSize;
        currentCardsToDraw.Value = baseCardsToDraw;
    }

    /// <summary>
    /// Modifies stamina (server-authoritative)
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void ModifyStaminaServerRpc(int amount)
    {
        currentStamina.Value = Mathf.Max(0, currentStamina.Value + amount);
    }

    /// <summary>
    /// Sets stamina to a specific value (server-authoritative)
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void SetStaminaServerRpc(int value)
    {
        currentStamina.Value = Mathf.Max(0, value);
    }

    /// <summary>
    /// Modifies strength (server-authoritative)
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void ModifyStrengthServerRpc(int amount)
    {
        currentStrength.Value = Mathf.Max(0, currentStrength.Value + amount);
    }

    /// <summary>
    /// Modifies defense (server-authoritative)
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void ModifyDefenseServerRpc(int amount)
    {
        currentDefense.Value = Mathf.Max(0, currentDefense.Value + amount);
    }

    /// <summary>
    /// Modifies weight (server-authoritative)
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void ModifyWeightServerRpc(int amount)
    {
        currentWeight.Value = Mathf.Max(0, currentWeight.Value + amount);
    }

    /// <summary>
    /// Modifies hand size (server-authoritative)
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void ModifyHandSizeServerRpc(int amount)
    {
        currentHandSize.Value = Mathf.Max(1, currentHandSize.Value + amount);
    }

    /// <summary>
    /// Initializes stats from a SumoCard. Sets base values and initializes current stats.
    /// </summary>
    public void InitializeFromSumoCard(SumoCard_Scriptable sumoCard)
    {
        if (!IsServer) return;
       
        baseStamina = sumoCard.sumoStamina;
        //baseWeight = sumoCard.SumoWeight;
       
        if (sumoCard.sumoAbility != null && sumoCard.sumoAbility is BasicAbility_Scriptable basicAbility)
        {
            ApplyAbilityToBaseStats(basicAbility);
        }        
    }

    /// <summary>
    /// Applies ability bonuses to base stats (called when Sumo is selected)
    /// </summary>
    private void ApplyAbilityToBaseStats(BasicAbility_Scriptable ability)
    {
        switch (ability.abilityType)
        {
            case BasicAbility_Scriptable.AbilityType.StrengthBonus:
                baseStrength += ability.abilityBonus;
                break;
            case BasicAbility_Scriptable.AbilityType.DefenseBonus:
                baseDefense += ability.abilityBonus;
                break;
            case BasicAbility_Scriptable.AbilityType.HandSizeBonus:
                baseHandSize += ability.abilityBonus;
                break;
            case BasicAbility_Scriptable.AbilityType.IncreaseWeightBonus:
                baseWeight += ability.abilityBonus;
                break;
        }
    }
}
