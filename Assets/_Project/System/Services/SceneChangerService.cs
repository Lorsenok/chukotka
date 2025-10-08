using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public interface ISceneChanger
{
    void ChangeScene(string sceneName, SceneStartType sceneStart, bool instantSkip = false);
}

public enum SceneStartType
{
    right, center, left
}

public class SceneChangerService : ISceneChanger
{
    public static Action<string> OnSceneChange { get; set; }
    public static SceneStartType SceneStart { get; private set; } = SceneStartType.center;
    
    public void ChangeScene(string sceneName, SceneStartType sceneStart = SceneStartType.center, bool instantSkip = false)
    {
        OnSceneChange?.Invoke(sceneName);
        SceneStart = sceneStart;
        if (instantSkip) SceneManager.LoadScene(sceneName);
    }
}