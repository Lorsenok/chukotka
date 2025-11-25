public class GatherTaskInstance : TaskInstance
{
    private int _current;
    private readonly GatherTaskConfig _config;

    public GatherTaskInstance(GatherTaskConfig config) : base(config)
    {
        _config = config;
    }

    public override void Start()
    {
        _current = 0;
        GameEvents.ItemCollected += OnItemCollected;
    }

    private void OnItemCollected(string itemId)
    {
        if (itemId == _config.targetItemId)
            _current++;
    }

    public override void Update() { }

    public override void Stop()
    {
        GameEvents.ItemCollected -= OnItemCollected;
    }

    public override bool IsCompleted => _current >= _config.requiredCount;
}