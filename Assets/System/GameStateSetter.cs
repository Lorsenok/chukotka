using UnityEngine;
using UnityEngine.Windows;
using Zenject;

public class GameStateSetter : MonoBehaviour
{
    [SerializeField] private GameState state;

    private IGameState gameState;
    [Inject]
    private void Init(IInputControler inputControler, IGameState gameState)
    {
        this.gameState = gameState;
    }

    private void Awake()
    {
        gameState.SetState(state);
    }
}
