using System;
using UnityEngine;
using Zenject;

public abstract class Minigame : MonoBehaviour
{
    public static Action OnMinigameEnd { get; set; }

    [SerializeField] protected GameState state = GameState.Mini;


    protected IGameState gameState;
    [Inject] private void Init(IGameState state)
    {
        gameState = state;
    }

    private GameState prevState = GameState.Game;

    public virtual void StartGame()
    {
        gameObject.SetActive(true);
        prevState = gameState.GetCurrectState();
        gameState.SetState(state);
    }

    public virtual void EndGame()
    {
        Destroy(gameObject);
        gameState.SetState(prevState);
        OnMinigameEnd?.Invoke();
    }
}
