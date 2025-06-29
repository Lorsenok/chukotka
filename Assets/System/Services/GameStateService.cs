using UnityEngine;

public enum GameState
{
    Menu,
    Game,
    Mini,
    Cutscene
}

public interface IGameState
{
    GameState GetCurrectState();
    void SetState(GameState state);
}

public class GameStateService : IGameState
{
    private GameState curState;

    public GameState GetCurrectState()
    {
        return curState;
    }

    public void SetState(GameState state)
    {
        curState = state;
    }
}