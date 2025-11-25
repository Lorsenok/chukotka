public class TalkTaskInstance : TaskInstance
{
    private string _npcId;
    private string _dialogId;
    private bool _done;

    public TalkTaskInstance(string npcId, string dialogId, string description)
    {
        _npcId = npcId;
        _dialogId = dialogId;
        _description = description;
        _taskType = TaskType.Talk;
    }

    public override void Start()
    {
        _done = false;
        GameEvents.DialogueCompleted += OnDialogueCompleted;
    }

    private void OnDialogueCompleted(string npcId)
    {
        //if (npcId == _cfg.NpcId)
            _done = true;
    }

    public override void Update() { }

    public override void Stop()
    {
        GameEvents.DialogueCompleted -= OnDialogueCompleted;
    }

    public override bool IsCompleted => _done;
}