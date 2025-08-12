using UnityEngine;

public interface ISaver
{
    void Save(string path, string data);
    string Load(string path);
    bool HasPath(string path);
}

public class SaverService : ISaver
{
    public void Save(string path, string data)
    {
        PlayerPrefs.SetString(path, data);
    }

    public string Load(string path)
    {
        return PlayerPrefs.GetString(path);
    }
    
    public bool HasPath(string path)
    {
        return PlayerPrefs.HasKey(path);
    }
}
