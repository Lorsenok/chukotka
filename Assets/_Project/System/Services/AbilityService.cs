using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityContainer
{
    List<AbilityObject> Abilities { get; }
    void AddAbility(AbilityObject ability);
    void Load();
    Action OnAbilitiesChanged { get; set; }
}

public class AbilityService : IAbilityContainer
{
    public List<AbilityObject> Abilities { get; private set; } = new();
    public Action  OnAbilitiesChanged { get; set; }
    
    public void AddAbility(AbilityObject ability)
    {
        Abilities.Add(ability);
        
        PlayerPrefs.SetInt("curabs", Abilities.Count);

        string id = Abilities.Count.ToString();
        PlayerPrefs.SetInt(id + "_jumps", ability.Jumps);
        PlayerPrefs.SetInt(id + "_combo", ability.Combo);
        PlayerPrefs.SetInt(id + "_dash", ability.DashPower);
        
        PlayerPrefs.SetFloat(id + "_ac", ability.Aceleration);
        PlayerPrefs.SetFloat(id + "_speed", ability.Speed);
        PlayerPrefs.SetFloat(id + "_dec", ability.Deceleration);
        
        OnAbilitiesChanged?.Invoke();
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey("curabs")) return;
        Abilities.Clear();
        for (int i = 0; i < PlayerPrefs.GetInt("curabs"); i++)
        {
            AbilityObject ability = new();
            
            ability.Jumps =  PlayerPrefs.GetInt(i.ToString() + "_jumps");
            ability.Combo = PlayerPrefs.GetInt(i.ToString() + "_combo");
            ability.DashPower = PlayerPrefs.GetInt(i.ToString() + "_dash");
            
            ability.Aceleration = PlayerPrefs.GetFloat(i.ToString() + "_ac");
            ability.Speed = PlayerPrefs.GetFloat(i.ToString() + "_speed");
            ability.Deceleration = PlayerPrefs.GetFloat(i.ToString() + "_dec");
            
            Abilities.Add(ability);
            OnAbilitiesChanged?.Invoke();
        }
    }
}
