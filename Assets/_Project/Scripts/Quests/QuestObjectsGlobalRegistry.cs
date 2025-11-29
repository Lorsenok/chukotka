using System;
using System.Collections.Generic;

public class QuestObjectsGlobalRegistry
{
    private List<QuestObjectStruct> _activeObjects = new();
    
    public event Action<QuestObjectStruct> OnObjectActivityChanged;
    
    public void ActivateObject(QuestObjectStruct questObject)
    {
        _activeObjects.Add(questObject);
        OnObjectActivityChanged?.Invoke(questObject);
    }
    
    public void DeactivateObject(QuestObjectStruct questObject)
    {
        _activeObjects.Remove(questObject);
        OnObjectActivityChanged?.Invoke(questObject);
    }
    
    public bool CheckObjectIsActive(QuestObjectStruct questObject)
    {
        return _activeObjects.Contains(questObject);
    }
}
