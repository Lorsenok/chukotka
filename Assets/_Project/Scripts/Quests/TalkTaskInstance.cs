public class TalkTaskInstance : TaskInstance
{
    private readonly TalkTaskConfig _cfg;
    private readonly NpcRegistry _registry;
    private bool _done;

    public TalkTaskInstance(TalkTaskConfig cfg, NpcRegistry registry) : base(cfg)
    {
        _cfg = cfg;
        _registry = registry;
    }

    public override void Start()
    {
        _done = false;
        GameEvents.DialogueCompleted += OnDialogueCompleted;
    }

    private void OnDialogueCompleted(string npcId)
    {
        if (npcId == _cfg.NpcId)
            _done = true;
    }

    public override void Update() { }

    public override void Stop()
    {
        GameEvents.DialogueCompleted -= OnDialogueCompleted;
    }

    public override bool IsCompleted => _done;
}