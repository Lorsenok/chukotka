using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PickableObject : MonoBehaviour
{
    protected InputSystem input;
    protected IGameState gameState;

    [Inject]
    private void Init(IInputControler inputControler, IGameState gameState)
    {
        input = inputControler.GetInputSystem();
        this.gameState = gameState;
    }

    protected bool canBePicked = false;

    public virtual void OnTriggerEnter(Collider other)
    {
        if (gameState.GetCurrectState() != GameState.Game) return;
        if (other.TryGetComponent(out Controler controler))
        {
            canBePicked = true;
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (gameState.GetCurrectState() != GameState.Game) return;
        if (other.TryGetComponent(out Controler controler))
        {
            canBePicked = false;
        }
    }

    public virtual void OnEnable()
    {
        input.Player.Use.performed += OnPickupButton;
    }

    public virtual void OnDisable()
    {
        input.Player.Use.performed -= OnPickupButton;
    }

    private bool isButtonPressed = false;
    private void OnPickupButton(InputAction.CallbackContext context)
    {
        isButtonPressed = !isButtonPressed;
        if (!isButtonPressed)
        {
            Action();
        }
    }

    public virtual void Action()
    {
        
    }
}
