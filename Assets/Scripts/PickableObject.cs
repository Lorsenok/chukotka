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

    public virtual void OnEnter(GameObject obj)
    {
        if (gameState.GetCurrectState() != GameState.Game) return;
        if (obj.TryGetComponent(out Controler controler))
        {
            canBePicked = true;
        }
    }

    public virtual void OnLeave(GameObject obj)
    {
        if (gameState.GetCurrectState() != GameState.Game) return;
        if (obj.TryGetComponent(out Controler controler))
        {
            canBePicked = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OnEnter(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnEnter(collision.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        OnLeave(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnLeave(collision.gameObject);
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
