using UnityEngine;
using Zenject;

public class QuestObjectController : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    [SerializeField] private QuestObjectType _type;
    [SerializeField] private SceneType sceneType;
    
    private QuestObjectsSceneRegistry _registry;
    public QuestObjectType Type => _type;
    public SceneType SceneType => sceneType;

    [Inject]
    public void Construct(QuestObjectsSceneRegistry registry)
    {
        _registry = registry;
    }

    private void OnEnable()
    {
        _registry.RegisterObjectController(this);
    }

    private void OnDisable()
    {
        _registry.UnregisterObjectController(this);
    }
    
    public void Activate()
    {
        _object.SetActive(true);
    }

    public void Deactivate()
    {
        _object.SetActive(false);
    }
}
