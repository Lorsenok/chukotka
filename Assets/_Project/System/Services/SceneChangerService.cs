using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public interface ISceneChanger
{
    void ChangeScene(string sceneName, SceneStartType sceneStart, bool instantSkip = false, bool saveTransition = true);
    void LoadGame();
}

public enum SceneStartType
{
    right = 0, center = 1, left = 2
}

public class SceneChangerService : ISceneChanger
{
    public static Action<string> OnSceneChange { get; set; }
    public static SceneStartType SceneStart { get; private set; } = SceneStartType.center;

    private static string SaveIgnore = "Menu"; // Костыль ванючий
    
    public void ChangeScene(string sceneName, SceneStartType sceneStart = SceneStartType.center, bool instantSkip = false, bool saveTransition = true)
    {
        bool sceneExists = false;
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);

            if (name == sceneName) sceneExists = true;
        }

        if (!sceneExists)
        {
            Debug.LogError("There is no scene by name: " +  sceneName);
            return;
        }
        
        OnSceneChange?.Invoke(sceneName);
        SceneStart = sceneStart;
        
        if (saveTransition && sceneName != SaveIgnore)
        {
            GameSaver.Save("sceneStart", (int)sceneStart);
            GameSaver.Save("sceneName", sceneName);
            GameSaver.GlobalSave();
        }
        
        if (instantSkip) SceneManager.LoadScene(sceneName);
    }

    public void LoadGame()
    {
        GameSaver.StopAllSaves = false;
        GameSaver.LoadGlobalSave();
        string sceneName = (string)GameSaver.Load("sceneName", typeof(string));
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("No last scene saves");
            return;
        }
        ChangeScene(
            sceneName,
            (SceneStartType)GameSaver.Load("sceneStart", typeof(int))
            );
        GameSaver.StopAllSaves = true;
    }
}