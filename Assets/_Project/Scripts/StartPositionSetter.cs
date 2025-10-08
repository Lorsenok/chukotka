using System;
using UnityEngine;

public class StartPositionSetter : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform posRight;
    [SerializeField] private Transform posCenter;
    [SerializeField] private Transform posLeft;

    private void Awake()
    {
        switch (SceneChangerService.SceneStart)
        {
            case SceneStartType.right: target.position = posRight.position; break;
            case SceneStartType.left: target.position = posLeft.position; break;
            case SceneStartType.center: target.position =  posCenter.position; break;
        }
    }
}
