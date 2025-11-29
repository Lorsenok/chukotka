using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Tasks/ActivateObjectsTask")]
public class ObjectsActivatorConfig : TaskConfig
{
    [Tooltip("Что надо активировать")] [SerializeField]
    private QuestObjectStruct[] _objectsToActivate;

    [Tooltip("Что надо деактивировать")] [SerializeField]
    private QuestObjectStruct[] _objectsToDeactivate;

    public QuestObjectStruct[] ObjectsToActivate => _objectsToActivate;
    public QuestObjectStruct[] ObjectsToDeactivate => _objectsToDeactivate;
}