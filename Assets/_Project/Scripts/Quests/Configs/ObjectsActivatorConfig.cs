using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Tasks/ActivateObjectsTask")]
public class ObjectsActivatorConfig : TaskConfig
{
    [Tooltip("Что надо активировать")] [SerializeField]
    private QuestObjectController[] _objectsToActivate;

    [Tooltip("Что надо деактивировать")] [SerializeField]
    private QuestObjectController[] _objectsToDeactivate;

    public QuestObjectController[] ObjectsToActivate => _objectsToActivate;
    public QuestObjectController[] ObjectsToDeactivate => _objectsToDeactivate;
}
