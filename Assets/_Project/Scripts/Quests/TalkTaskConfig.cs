using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Tasks/TalkTask")]
public class TalkTaskConfig : TaskConfig
{
    [SerializeField] private string _npcId;
    [SerializeField] private string dialogId;

    public string NpcId => _npcId;
    public string DialogId => dialogId;
    
    public override TaskInstance CreateInstance() => new TalkTaskInstance(this);
}