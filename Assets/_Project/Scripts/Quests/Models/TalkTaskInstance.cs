using UnityEngine;

public class TalkTaskInstance : TaskInstance
{
    private readonly QuestDialogEvents _dialogEvents;
    
    private string _npcId;
    private string _dialogId;
    private bool _done;

    public string NpcId => _npcId;
    public string DialogId => _dialogId;
    public bool Done => _done;

    public TalkTaskInstance(QuestDialogEvents dialogEvents, string npcId, string dialogId, string description)
    {
        _dialogEvents = dialogEvents;
        _npcId = npcId;
        _dialogId = dialogId;
        _description = description;
        _taskType = TaskType.Talk;
    }

    public override void Start()
    {
        _done = false;
        _dialogEvents.ActivateDialog(this);
        DialogueService.OnDialogueEnd += CheckDialogCompleted;
        
        Debug.Log("активирован диалог" + _dialogId + " для NPC" + _npcId + _description);
    }

    public override void Update() { }

    public override void Stop()
    {
        DialogueService.OnDialogueEnd -= CheckDialogCompleted;
        Debug.Log("Завершён диалог" + _dialogId + " для NPC" + _npcId + _description);
    }

    public override bool IsCompleted => _done;

    private void CheckDialogCompleted(TalkTaskInstance instance)
    {
        _dialogEvents.CompleteDialog(this);
        _done = true;
        Complete();
    }
}