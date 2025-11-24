using System;
using System.Collections.Generic;
using UnityEngine;
public interface IAbilityContainer 
{ 
    List<AbilityObject> Abilities { get; } 
    void AddAbility(AbilityObject ability); 
    Action OnAbilitiesChanged { get; set; } 
}
public class AbilityService : IAbilityContainer 
{ 
    public List<AbilityObject> Abilities { get; private set; } = new(); 
    public Action OnAbilitiesChanged { get; set; } 

    public void AddAbility(AbilityObject ability)
    {
        Abilities.Add(ability);
        OnAbilitiesChanged?.Invoke(); 
    }
}