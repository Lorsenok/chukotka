using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PickableObject : MonoBehaviour
{
    protected GameObject target;

    protected InputSystem input;
    protected IGameState gameState;

    protected Collider2D coll;

    [Inject]
    private void Init(IInputControler inputControler, IGameState gameState)
    {
        input = inputControler.GetInputSystem();
        this.gameState = gameState;
    }

    protected bool canBePicked = false;

    public virtual void OnEnter(GameObject obj)
    {
        if (gameState.GetCurrentState() != GameState.Game) return;
        if (obj == target)
        {
            canBePicked = true;
        }
    }

    public virtual void OnLeave(GameObject obj)
    {
        if (gameState.GetCurrentState() != GameState.Game) return;
        if (obj == target)
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

    public virtual void Start()
    {
        coll = GetComponent<Collider2D>();
        target = FindObjectsByType<Controller>(FindObjectsSortMode.None)[0].gameObject;
    }
}
