using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameMenu : MonoBehaviour
{
    [Header("Game Menu")]
    [SerializeField] protected MenuManager menuManager;
    [SerializeField] protected Menu blankMenu;
    [SerializeField] protected Menu defaultMenu;
    [SerializeField] protected GameButton openButton;
    [SerializeField] protected InputAction inputAction;
    [SerializeField] protected GameState state = GameState.Any;

    protected IGameState gameState;
    protected InputSystem inputSystem;
    [Inject] private void Init(IGameState gameState, IInputControler input)
    {
        this.gameState = gameState;
        inputSystem = input.GetInputSystem();
    }

    protected GameState curState = GameState.Game;

    public virtual void Update()
    {
        if (!isMenuOpen && blankMenu.Open) curState = gameState.GetCurrectState();
    }

    public virtual void OnEnable()
    {
        inputAction.performed += Open;
        if (openButton != null) openButton.OnButtonPressed += OpenByButton;
    }

    public virtual void OnDisable()
    {
        inputAction.performed -= Open;
        if (openButton != null) openButton.OnButtonPressed -= OpenByButton;
    }

    private bool isMenuOpen = false;

    public virtual void Open(InputAction.CallbackContext context)
    {
        isMenuOpen = !isMenuOpen;

        menuManager.MenuOpen(isMenuOpen ? blankMenu.MenuName : defaultMenu.MenuName);
        if (state != GameState.Any) gameState.SetState(isMenuOpen ? curState : state);
    }

    protected virtual void OpenByButton()
    {
        Open(new InputAction.CallbackContext());
    }
}
