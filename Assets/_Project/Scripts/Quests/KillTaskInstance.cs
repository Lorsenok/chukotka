public class KillTaskInstance : TaskInstance
{
    private int _current;
    private readonly KillTaskConfig _config;

    public KillTaskInstance(KillTaskConfig config) : base(config)
    {
        _config = config;
    }

    public override void Start()
    {
        _current = 0;
        GameEvents.EnemyKilled += OnEnemyKilled;
    }

    private void OnEnemyKilled(string enemyId)
    {
        if (enemyId == _config.targetEnemyId)
            _current++;
    }

    public override void Update() { /* ничего не нужно — событие обновляет состояние */ }

    public override void Stop()
    {
        GameEvents.EnemyKilled -= OnEnemyKilled;
    }

    public override bool IsCompleted => _current >= _config.requiredCount;
}