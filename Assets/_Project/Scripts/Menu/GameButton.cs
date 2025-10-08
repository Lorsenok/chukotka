using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

public enum GameButtonAction
{
    Nothing,
    StartScene,
    ChangeMenu,
    ExitGame
}

public class GameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Action OnButtonPressed { get; set; }

    [SerializeField] protected GameButtonAction action;
    [SerializeField] protected string index;
    [SerializeField] protected MenuManager menuManager;

    protected bool isMouseOn = false;

    protected InputSystem inputSystem;
    protected ISceneChanger sceneChanger;
    [Inject] public void Init(IInputControler controler, ISceneChanger sceneChanger)
    {
        inputSystem = controler.GetInputSystem();
        this.sceneChanger = sceneChanger;
    }

    public virtual void OnMouseEnter()
    {
        isMouseOn = true;
    }

    public virtual void OnMouseExit()
    {
        isMouseOn = false;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit();
    }

    private bool hasStarted = false;
    public virtual void Start()
    {
        inputSystem.UI.Click.performed += OnClick;
        hasStarted = true;
    }

    public virtual void OnEnable()
    {
        if (hasStarted) inputSystem.UI.Click.performed += OnClick;
    }

    public virtual void OnDisable()
    {
        inputSystem.UI.Click.performed -= OnClick;
    }

    protected bool isHolding = false;
    public virtual void OnClick(InputAction.CallbackContext context)
    {
        isHolding = !isHolding;
        if (!isHolding) Action();
    }

    public virtual void Action()
    {
        if (!isMouseOn) return;

        switch (action)
        {
            case GameButtonAction.StartScene:
                sceneChanger.ChangeScene(index, SceneStartType.center); break;
            case GameButtonAction.ChangeMenu:
                menuManager.MenuOpen(index); break;
            case GameButtonAction.ExitGame:
                Application.Quit(); break;
        }

        OnButtonPressed?.Invoke();
    }
}
