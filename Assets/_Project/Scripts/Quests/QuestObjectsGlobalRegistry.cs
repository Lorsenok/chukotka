using System;
using System.Collections.Generic;

public class QuestObjectsGlobalRegistry
{
    private List<QuestObjectController> _activeObjects = new();
    
    public event Action<QuestObjectController> OnObjectActivityChanged;
    
    public void ActivateObject(QuestObjectController questObject)
    {
        _activeObjects.Add(questObject);
    }
    
    public void DeactivateObject(QuestObjectController questObject)
    {
        _activeObjects.Remove(questObject);
    }
    
    public bool CheckObjectIsActive(QuestObjectController questObject)
    {
        return _activeObjects.Contains(questObject);
    }
}
