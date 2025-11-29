using UnityEngine;

public class QuestObjectController : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    [SerializeField] private QuestObjectType _type;
    [SerializeField] private SceneType sceneType;
    
    public QuestObjectType Type => _type;
    public SceneType SceneType => sceneType;

    public void Activate()
    {
        _object.SetActive(true);
    }

    public void Deactivate()
    {
        _object.SetActive(false);
    }
}
