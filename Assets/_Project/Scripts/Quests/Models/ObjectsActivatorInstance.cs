public class ObjectsActivatorInstance : TaskInstance
{
    private QuestObjectsGlobalRegistry _questObjectsGlobalRegistry;
    
    private QuestObjectStruct[] _objectsToActivate;
    private QuestObjectStruct[] _objectsToDeactivate;
    
    private bool _done;

    public ObjectsActivatorInstance(QuestObjectsGlobalRegistry questObjectsGlobalRegistry,
        QuestObjectStruct[] objectsToActivate, QuestObjectStruct[] objectsToDeactivate)
    {
        _questObjectsGlobalRegistry = questObjectsGlobalRegistry;
        _objectsToActivate = objectsToActivate;
        _objectsToDeactivate = objectsToDeactivate;
    }
    
    public override void Start()
    {
        foreach (var obj in _objectsToActivate)
        {
            _questObjectsGlobalRegistry.ActivateObject(obj);
        }
        
        foreach (var obj in _objectsToDeactivate)
        {
            _questObjectsGlobalRegistry.DeactivateObject(obj);
        }
        
        _done = true;
    }

    public override void Update() { }

    public override void Stop() { }

    public override bool IsCompleted => _done;
}
