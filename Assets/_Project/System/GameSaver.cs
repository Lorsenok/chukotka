using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameSaver
{
    private const string globalSaveIndex = "globalSave_";
    private const string globalKeysKey = "globalSaveKeys";
    private const string globalTypesKey = "globalSaveTypes";

    private static List<string> globalSaveKeys = new List<string>();
    private static List<Type> globalSaveTypes = new List<Type>();

    static GameSaver()
    {
        LoadGlobalSaveMetadata();
    }

    private static bool LocalSave(string key, object value)
    {
        switch (value)
        {
            case int i:
                PlayerPrefs.SetInt(key, i);
                break;

            case float f:
                PlayerPrefs.SetFloat(key, f);
                break;

            case string s:
                PlayerPrefs.SetString(key, s);
                break;

            default:
                Debug.LogError($"Unknown object type for save: {value.GetType().Name}. Only Int32, Single, and String are supported.");
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
            SaveGlobalSaveMetadata();
        }
    }

    public static object Load(string key, Type type)
    {
        switch (type.Name)
        {
            case "Int32":
                return PlayerPrefs.GetInt(key);

            case "Single":
                return PlayerPrefs.GetFloat(key);

            case "String":
                return PlayerPrefs.GetString(key);

            default:
                Debug.LogError($"Unknown object type for load: {type.Name}. Only Int32, Single, and String are supported.");
                return null;
        }
    }

    public static void GlobalSave()
    {
        for (int i = 0; i < globalSaveKeys.Count; i++)
        {
            LocalSave(globalSaveIndex + globalSaveKeys[i], Load(globalSaveKeys[i], globalSaveTypes[i]));
        }

        PlayerPrefs.Save();
    }

    public static void LoadGlobalSave()
    {
        for (int i = 0; i < globalSaveKeys.Count; i++)
        {
            object value = Load(globalSaveIndex + globalSaveKeys[i], globalSaveTypes[i]);
            Save(globalSaveKeys[i], value);
        }

        PlayerPrefs.Save();
    }

    private static void SaveGlobalSaveMetadata()
    {
        string keysSerialized = string.Join("|", globalSaveKeys);
        string typesSerialized = string.Join("|", globalSaveTypes.ConvertAll(t => t.AssemblyQualifiedName));

        PlayerPrefs.SetString(globalKeysKey, keysSerialized);
        PlayerPrefs.SetString(globalTypesKey, typesSerialized);
        PlayerPrefs.Save();
    }

    private static void LoadGlobalSaveMetadata()
    {
        string keysSerialized = PlayerPrefs.GetString(globalKeysKey, "");
        string typesSerialized = PlayerPrefs.GetString(globalTypesKey, "");

        if (string.IsNullOrEmpty(keysSerialized) || string.IsNullOrEmpty(typesSerialized))
            return;

        string[] keys = keysSerialized.Split('|');
        string[] typeNames = typesSerialized.Split('|');

        if (keys.Length != typeNames.Length)
        {
            Debug.LogWarning("Global save metadata corrupted: keys and types count mismatch.");
            return;
        }

        globalSaveKeys.Clear();
        globalSaveTypes.Clear();

        for (int i = 0; i < keys.Length; i++)
        {
            if (string.IsNullOrEmpty(keys[i])) continue;

            Type t = Type.GetType(typeNames[i]);
            if (t == null)
            {
                Debug.LogWarning($"Unknown type '{typeNames[i]}' in saved metadata, skipping key '{keys[i]}'.");
                continue;
            }

            globalSaveKeys.Add(keys[i]);
            globalSaveTypes.Add(t);
        }
    }
}
