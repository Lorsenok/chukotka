using System;
using System.Collections.Generic;
using UnityEngine;
public interface IAbilityContainer 
{ 
    List<AbilityObject> Abilities { get; } 
    void AddAbility(AbilityObject ability); 
    void Load(); 
    bool HasAbility(Predicate<AbilityObject> predicate); 
    Action OnAbilitiesChanged { get; set; } 
}
public class AbilityService : IAbilityContainer 
{ 
    public List<AbilityObject> Abilities { get; private set; } = new(); 
    public Action OnAbilitiesChanged { get; set; } 
    public bool HasAbility(Predicate<AbilityObject> predicate) 
    { 
        return Abilities.Exists(predicate); 
    } 
    public void AddAbility(AbilityObject ability) { if (ability == null) return; 
     if (HasAbility(a => a.Jumps == ability.Jumps && a.DashPower == ability.DashPower && a.Combo == ability.Combo)) return; 
        Abilities.Add(ability); int idIndex = Abilities.Count - 1; 
        GameSaver.Save("curabs", Abilities.Count); 
        GameSaver.Save(idIndex + "_jumps", ability.Jumps); 
        GameSaver.Save(idIndex + "_combo", ability.Combo); 
        GameSaver.Save(idIndex + "_dash", ability.DashPower);
        GameSaver.Save(idIndex + "_ac", ability.Aceleration); 
        GameSaver.Save(idIndex + "_speed", ability.Speed); 
        GameSaver.Save(idIndex + "_dec", ability.Deceleration); 
        OnAbilitiesChanged?.Invoke(); 
    } 
    public void Load() 
    { 
        Abilities.Clear(); 
        if (!PlayerPrefs.HasKey("curabs")) 
        { 
            OnAbilitiesChanged?.Invoke(); 
            return; 
        } 
        int count = PlayerPrefs.GetInt("curabs"); 
        for (int i = 0; i < count; i++) 
        { 
            AbilityObject ability = new AbilityObject 
            { 
                Jumps = (int)GameSaver.Load(i + "_jumps", typeof(int)), 
                Combo = (int)GameSaver.Load(i + "_combo", typeof(int)), 
                DashPower = (int)GameSaver.Load(i + "_dash", typeof(int)), 
                Aceleration = (float)GameSaver.Load(i + "_ac", typeof(float)), 
                Speed = (float)GameSaver.Load(i + "_speed", typeof(float)), 
                Deceleration = (float)GameSaver.Load(i + "_dec", typeof(float)) }; 
                Abilities.Add(ability); } OnAbilitiesChanged?.Invoke(); 
    } 
}