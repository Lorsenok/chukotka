using UnityEngine;

// Конфиг: какие враги и сколько.
[CreateAssetMenu(menuName = "Quest/Tasks/KillTask")]
public class KillTaskConfig : TaskConfig
{
    public string targetEnemyId;
    public int requiredCount = 1;

}