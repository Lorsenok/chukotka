using UnityEngine;

public abstract class TaskConfig : ScriptableObject
{
    [TextArea] public string Description;
    public TaskType TaskType;
}

public enum TaskType
{
    Talk,
    Collect,
    Kill,
    SetInventory,
    Ability
}