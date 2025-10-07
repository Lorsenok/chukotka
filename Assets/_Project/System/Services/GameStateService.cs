using UnityEngine;

public enum GameState
{
    Menu,
    Game,
    Mini,
    Cutscene,
    Any
}

public interface IGameState
{
    GameState GetCurrentState();
    void SetState(GameState state);
}

public class GameStateService : IGameState
{
    private GameState curState;

    public GameState GetCurrentState()
    {
        return curState;
    }

    public void SetState(GameState state)
    {
        curState = state;
    }
}