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
    [SerializeField] protected bool switchByEscape;
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
        if (switchByEscape) inputSystem.UI.Menu.performed += OpenBySwitch;
        if (openButton != null) openButton.OnButtonPressed += Open;
    }

    public virtual void OnDisable()
    {
        if (switchByEscape) inputSystem.UI.Menu.performed -= OpenBySwitch;
        if (openButton != null) openButton.OnButtonPressed -= Open;
    }

    private bool isMenuOpen = false;

    private bool hasSwitched = false;
    protected virtual void OpenBySwitch(InputAction.CallbackContext context)
    {
        hasSwitched = !hasSwitched;

        if (hasSwitched) Open();
    }

    public virtual void Open()
    {
        isMenuOpen = !isMenuOpen;

        menuManager.MenuOpen(isMenuOpen ? blankMenu.MenuName : defaultMenu.MenuName);
        if (state != GameState.Any) gameState.SetState(isMenuOpen ? curState : state);
    }
}
