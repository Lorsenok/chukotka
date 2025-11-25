using System;

public abstract class TaskInstance : IDisposable
{
    public readonly TaskConfig Config;

    protected TaskInstance(TaskConfig config)
    {
        Config = config;
    }

    // Вызывается при старте задачи.
    public abstract void Start();

    // Прогресс/периодические проверки (если нужно).
    public abstract void Update();

    // Принудительно остановить задачу (для отмены/рестарта).
    public abstract void Stop();

    // Является ли задача завершённой.
    public abstract bool IsCompleted { get; }

    // Для отписки от событий и очистки.
    public virtual void Dispose() => Stop();
}