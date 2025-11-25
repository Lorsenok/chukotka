using System;
using System.Collections.Generic;

public class QuestInstance : IDisposable
{
    private readonly QuestConfig _config;
    private readonly List<TaskInstance> _taskInstances;
    private int _currentTaskIndex = -1;

    public QuestInstance(QuestConfig config)
    {
        _config = config;
        _taskInstances = new List<TaskInstance>(_config.tasks.Count);
        foreach (var t in _config.tasks)
            _taskInstances.Add(t.CreateInstance());
    }

    public void Start()
    {
        _currentTaskIndex = -1;
        MoveNext();
    }

    private void MoveNext()
    {
        // Stop previous if any
        if (_currentTaskIndex >= 0 && _currentTaskIndex < _taskInstances.Count)
        {
            var prev = _taskInstances[_currentTaskIndex];
            prev.Dispose();
        }

        _currentTaskIndex++;

        if (_currentTaskIndex < _taskInstances.Count)
        {
            var current = _taskInstances[_currentTaskIndex];
            current.Start();
        }
    }

    public void Update()
    {
        if (_currentTaskIndex < 0 || _currentTaskIndex >= _taskInstances.Count)
            return;

        var current = _taskInstances[_currentTaskIndex];
        current.Update();

        if (current.IsCompleted)
            MoveNext();
    }

    public bool IsCompleted => _currentTaskIndex >= _taskInstances.Count;

    public void Stop()
    {
        if (_currentTaskIndex >= 0 && _currentTaskIndex < _taskInstances.Count)
            _taskInstances[_currentTaskIndex].Stop();
    }

    public void Dispose()
    {
        foreach (var t in _taskInstances)
            t.Dispose();
    }
}