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
    }

    private void OnDialogCompleted(TalkTaskInstance instance)
    {
        //if (npcId == _cfg.NpcId)
            _done = true;
    }

    public override void Update() { }

    public override void Stop()
    {
        _dialogEvents.OnDialogCompleted -= OnDialogCompleted;
    }

    public override bool IsCompleted => _done;
}