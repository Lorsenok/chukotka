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
    private const string SAVE_KEY = "Tasks";
    
    public Action OnTasksChanged { get; set; }

    private void OnChange()
    {
        string json = JsonUtility.ToJson(Tasks);
        GameSaver.Save(SAVE_KEY, json);
    }
    
    public TaskService()
    {
        OnTasksChanged += OnChange;
        
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = (string)GameSaver.Load(SAVE_KEY, typeof(string));
            List<Task> taskList = JsonUtility.FromJson<List<Task>>(json);
            Tasks = taskList ?? new List<Task>();
        }
    }

    public List<Task> Tasks { get; set; }
}