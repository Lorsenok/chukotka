using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private Menu blankMenu;
    [SerializeField] private Menu defaultMenu;
    [SerializeField] private GameButton continueButton;

    private IGameState gameState;
    private InputSystem inputSystem;
    [Inject] private void Init(IGameState gameState, IInputControler input)
    {
        this.gameState = gameState;
        inputSystem = input.GetInputSystem();
    }

    private GameState curState = GameState.Game;

    private void Update()
    {
        if (!isMenuOpen && blankMenu.Open) curState = gameState.GetCurrectState();
    }

    private void OnEnable()
    {
        inputSystem.UI.Menu.performed += Open;
        continueButton.OnButtonPressed += OpenByButton;
    }

    private void OnDisable()
    {
        inputSystem.UI.Menu.performed -= Open;
        continueButton.OnButtonPressed -= OpenByButton;
    }

    private bool isMenuOpen = false;
    private bool isButtonPressed = false;

    private void Open(InputAction.CallbackContext context)
    {
        isButtonPressed = !isButtonPressed;
        if (!isButtonPressed) return;

        isMenuOpen = !isMenuOpen;

        menuManager.MenuOpen(isMenuOpen ? blankMenu.MenuName : defaultMenu.MenuName);
        gameState.SetState(isMenuOpen ? curState : GameState.Menu);
    }

    private void OpenByButton()
    {
        Open(new InputAction.CallbackContext());
    }
}
