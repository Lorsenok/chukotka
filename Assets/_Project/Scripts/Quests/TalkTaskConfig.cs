using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Tasks/TalkTask")]
public class TalkTaskConfig : TaskConfig
{
    public string npcId;

    public override TaskInstance CreateInstance() => new TalkTaskInstance(this);
}