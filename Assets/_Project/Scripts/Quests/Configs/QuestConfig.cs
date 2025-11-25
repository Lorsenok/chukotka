using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/QuestConfig")]
public class QuestConfig : ScriptableObject
{
    public string questId;
    public string questDescription;
    public List<TaskConfig> tasks = new List<TaskConfig>();
}