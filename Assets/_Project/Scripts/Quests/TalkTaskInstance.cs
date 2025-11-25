public class TalkTaskInstance : TaskInstance
{
    private readonly TalkTaskConfig _cfg;
    private bool _done;

    public TalkTaskInstance(TalkTaskConfig cfg) : base(cfg)
    {
        _cfg = cfg;
    }

    public override void Start()
    {
        _done = false;
        GameEvents.DialogueCompleted += OnDialogueCompleted;
    }

    private void OnDialogueCompleted(string npcId)
    {
        if (npcId == _cfg.npcId)
            _done = true;
    }

    public override void Update() { }

    public override void Stop()
    {
        GameEvents.DialogueCompleted -= OnDialogueCompleted;
    }

    public override bool IsCompleted => _done;
}