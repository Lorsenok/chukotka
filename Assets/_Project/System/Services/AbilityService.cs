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
        
        GameSaver.Save("curabs", Abilities.Count);

        string id = Abilities.Count.ToString();
        GameSaver.Save(id + "_jumps", ability.Jumps);
        GameSaver.Save(id + "_combo", ability.Combo);
        GameSaver.Save(id + "_dash", ability.DashPower);
        
        GameSaver.Save(id + "_ac", ability.Aceleration);
        GameSaver.Save(id + "_speed", ability.Speed);
        GameSaver.Save(id + "_dec", ability.Deceleration);
        
        OnAbilitiesChanged?.Invoke();
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey("curabs")) return;
        Abilities.Clear();
        for (int i = 0; i < PlayerPrefs.GetInt("curabs"); i++)
        {
            AbilityObject ability = new();
            
            ability.Jumps =  (int)GameSaver.Load(i.ToString() + "_jumps", typeof(int));
            ability.Combo = (int)GameSaver.Load(i.ToString() + "_combo", typeof(int));
            ability.DashPower = (int)GameSaver.Load(i.ToString() + "_dash", typeof(int));
            
            ability.Aceleration = (float)GameSaver.Load(i.ToString() + "_ac", typeof(float));
            ability.Speed = (float)GameSaver.Load(i.ToString() + "_speed", typeof(float));
            ability.Deceleration = (float)GameSaver.Load(i.ToString() + "_dec", typeof(float));
            
            Abilities.Add(ability);
            OnAbilitiesChanged?.Invoke();
        }
    }
}
