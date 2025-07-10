using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Task", menuName = "Scriptable Objects/Task")]
public class Task : ScriptableObject
{
    public int taskId;
    public LocalizedString description;
}
