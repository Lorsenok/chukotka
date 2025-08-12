using UnityEngine;
using Zenject;

public class BootstrapSceneChanger : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Menu";
    private ISceneChanger sceneChanger;

    [Inject] private void Init(ISceneChanger sceneChanger)
    {
        this.sceneChanger = sceneChanger;
    }

    private void Start()
    {
        sceneChanger.ChangeScene(nextSceneName);
    }
}
