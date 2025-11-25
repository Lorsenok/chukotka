using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Tasks/GatherTask")]
public class GatherTaskConfig : TaskConfig
{
    public string targetItemId;
    public int requiredCount = 1;

    public override TaskInstance CreateInstance() => new GatherTaskInstance(this);
}