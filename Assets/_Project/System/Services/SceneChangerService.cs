using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public interface ISceneChanger
{
    void ChangeScene(string sceneName, bool instantSkip = false);
}

public class SceneChangerService : ISceneChanger
{
    public static Action<string> OnSceneChange { get; set; }

    public void ChangeScene(string sceneName, bool instantSkip = false)
    {
        OnSceneChange?.Invoke(sceneName);
        if (instantSkip) SceneManager.LoadScene(sceneName);
    }
}