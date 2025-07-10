using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITaskContainer
{
    List<Task> Tasks { get; set; }
    Action OnTasksChanged { get; set; }
}

public class TaskService : ITaskContainer
{
    public Action OnTasksChanged { get; set; }
    public List<Task> Tasks { get; set; } = new();
}