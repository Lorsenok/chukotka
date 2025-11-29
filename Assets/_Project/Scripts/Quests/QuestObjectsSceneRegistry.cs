using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class QuestObjectsSceneRegistry : MonoBehaviour
{
    [SerializeField] private SceneType sceneType;
    
    private readonly Dictionary<QuestObjectType, QuestObjectController> _objects = new();
    
    [Inject] 
    private readonly QuestObjectsGlobalRegistry _globalRegistry;

    private void Start()
    {
        _globalRegistry.OnObjectActivityChanged += SetObjectActive;
     
        SetAllObjectsActive();
    }

    private void OnDisable()
    {
        _globalRegistry.OnObjectActivityChanged -= SetObjectActive;
    }

    public void RegisterObjectController(QuestObjectController questObject)
    {
        _objects.Add(questObject.Type, questObject);
    }

    public void UnregisterObjectController(QuestObjectController questObject)
    {
        _objects.Remove(questObject.Type);
    }
    
    private void SetAllObjectsActive()
    {
        foreach (var questObject in _objects)
        {
            SetObjectActive(questObject.Value);
        }
    }

    private void SetObjectActive(QuestObjectController questObject)
    {
        if (questObject.SceneType != sceneType)
            return;
        
        if (_globalRegistry.CheckObjectIsActive(questObject))
            questObject.Activate();
        else
            questObject.Deactivate();
    }
}
