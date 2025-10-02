using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "Scriptable Objects/Ability")]
public class Ability : ScriptableObject
{
    public int jumps = 0;
    public int combo = 0;
    public int dashPower = 0;
    
    public float aceleration = 0f;
    public float speed = 0f;
    public float deceleration = 0f;
}

public class AbilityObject
{
    public void SetAbility(Ability ability)
    {
        Jumps = ability.jumps;
        Combo = ability.combo;
        DashPower = ability.dashPower;
        
        Aceleration = ability.aceleration;
        Speed = ability.speed;
        Deceleration = ability.deceleration;
    }
    
    public int Jumps { get; set; }
    public int Combo { get; set; }
    public int DashPower { get; set; }
    
    public float Aceleration { get; set; }
    public float Speed { get; set; }
    public float Deceleration { get; set; }
}