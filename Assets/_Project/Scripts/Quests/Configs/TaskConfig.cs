using UnityEngine;

public abstract class TaskConfig : ScriptableObject
{
    [TextArea] public string Description;
    public TaskType TaskType;
}