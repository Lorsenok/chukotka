using System;
using System.Collections.Generic;

public class QuestInstance : IDisposable
{
    private readonly List<TaskInstance> _taskInstances;
    private int _currentTaskIndex = -1;
    private string _id;
    private string _description;

    public QuestInstance(string id,  string description, List<TaskInstance> taskInstances)
    {
        _id = id;
        _description = description;
        _taskInstances = new List<TaskInstance>(taskInstances);
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
            prev.OnCompleted -= OnTaskCompleted;
            prev.Dispose();
        }

        _currentTaskIndex++;

        if (_currentTaskIndex < _taskInstances.Count)
        {
            var current = _taskInstances[_currentTaskIndex];
            current.OnCompleted += OnTaskCompleted;
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
    
    private void OnTaskCompleted(TaskInstance task)
    {
        MoveNext();
    }
}