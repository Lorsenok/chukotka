using System;

public interface IGameLayerChanger
{
    void ChangeLayer(int id, bool instantSkip = false);
}

public class GameLayerChangerService : IGameLayerChanger
{
    public static Action<int> OnLayerChange { get; set; }

    public void ChangeLayer(int id, bool instantSkip = false)
    {
        OnLayerChange?.Invoke(id);
    }
}