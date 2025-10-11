using System;
using UnityEngine;
using Zenject;

public class SceneSwitchTrigger : Trigger
{
    [Header("Scene Switch")]
    [SerializeField] private string nextScene;
    [SerializeField] private SceneStartType sceneStartType;

    private ISceneChanger sceneChanger;
    [Inject]
    private void Init(ISceneChanger sceneChanger)
    {
        this.sceneChanger = sceneChanger;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Controller>())
        {
            enabled = false;
            Action();
        }
    }

    public override void Action()
    {
        sceneChanger.ChangeScene(nextScene, sceneStartType);
        base.Action();
    }
}
