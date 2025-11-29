using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class QuestObjectsSceneRegistry : MonoBehaviour
{
    [SerializeField] private SceneType sceneType;

    private readonly Dictionary<QuestObjectType, QuestObjectController> _objects = new();

    [Inject] private readonly QuestObjectsGlobalRegistry _globalRegistry;

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
            var questObjectStruct = GetQuestObjectStruct(questObject.Value);
            SetObjectActive(questObjectStruct);
        }
    }

    private void SetObjectActive(QuestObjectStruct questObjectStruct)
    {
        if (questObjectStruct.SceneType != sceneType)
            return;

        if (_globalRegistry.CheckObjectIsActive(questObjectStruct))
            _objects[questObjectStruct.QuestObjectType].Activate();
        else
            _objects[questObjectStruct.QuestObjectType].Deactivate();
    }

    private QuestObjectStruct GetQuestObjectStruct(QuestObjectController controller)
    {
        QuestObjectStruct questObjectStruct = new QuestObjectStruct
        {
            SceneType = sceneType,
            QuestObjectType = controller.Type
        };
        return questObjectStruct;
    }
}