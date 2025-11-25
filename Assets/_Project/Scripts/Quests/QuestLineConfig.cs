using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/QuestLine")]
public class QuestLineConfig : ScriptableObject
{
    public List<QuestConfig> quests = new List<QuestConfig>();
}