using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameSaver
{
    private const string globalSaveIndex = "globalSave_";
    private static List<string> globalSaveKeys = new List<string>();
    private static List<Type> globalSaveTypes = new List<Type>();

    private static bool LocalSave(string key, object value)
    {
        switch (value.GetType().Name)
        {
            case "Int32":
                PlayerPrefs.SetInt(key, (int) value);
                break;

            case "Single": // float
                PlayerPrefs.SetFloat(key, (float) value);
                break;

            case "String":
                PlayerPrefs.SetString(key, (string) value);
                break;

            default:
                Debug.LogError("Unknown object type for save, you can save only Int32, Single and String");
                return false;
        }

        return true;
    }
    
    public static void Save(string key, object value)
    {
        bool isSaveDone = LocalSave(key, value);
        if (!isSaveDone) return;

        if (!globalSaveKeys.Contains(key))
        {
            globalSaveKeys.Add(key);
            globalSaveTypes.Add(value.GetType());
        }
    }

    public static object Load(string key, Type type)
    {
        switch (type.Name)
        {
            case "Int32":
                return PlayerPrefs.GetInt(key);

            case "Single": // float
                return PlayerPrefs.GetFloat(key);

            case "String":
                return PlayerPrefs.GetString(key);
            
            default:
                Debug.LogError("Unknown object type for load, you can load only Int32, Single and String");
                break;
        }

        return null;
    }

    public static void GlobalSave()
    {
        for (int i = 0; i < globalSaveKeys.Count; i++)
        {
            LocalSave(globalSaveIndex + globalSaveKeys[i], globalSaveTypes[i]);
        }
    }

    public static void LoadGlobalSave()
    {
        for (int i = 0; i < globalSaveKeys.Count; i++)
        {
            Save(globalSaveKeys[i], Load(globalSaveIndex + globalSaveKeys[i], globalSaveTypes[i]));
        }
    }
}
