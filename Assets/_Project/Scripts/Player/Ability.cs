using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "Scriptable Objects/Task")]
public class Ability : ScriptableObject
{
    public int finalJumps = 0;
        
    public float finalAc = 0f;
    public float finalSpeed = 0f;
    public float finalDec = 0f;
}
