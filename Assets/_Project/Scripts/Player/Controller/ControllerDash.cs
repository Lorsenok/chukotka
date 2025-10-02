using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class ControllerDash : ControllerAddition
{
    [SerializeField] private Rigidbody2D rg;
    [SerializeField] private float rgDeadZone = 0.1f;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashForceExpireSpeed;
    [SerializeField] private Timer dashTimer;
    private float curDashPower = 0f;
    
    private InputSystem input;
    private IGameState gameState;
    [Inject] private void Init(IInputControler inputControler, IGameState gameState)
    {
        input = inputControler.GetInputSystem();
        this.gameState = gameState;
    }

    private void OnEnable()
    {
        input.Player.Dash.performed += Dash;
        dashTimer.OnTimerEnd += OnDashBlock;
    }

    private void OnDisable()
    {
        input.Player.Dash.performed -= Dash;
        dashTimer.OnTimerEnd -= OnDashBlock;
    }

    private void Update()
    {
        if (rg.linearVelocityY == 0f) canDash = true;
        curDashPower = Mathf.Lerp(curDashPower, 0f, Time.deltaTime * dashForceExpireSpeed);

        AdditionalSpeed = new Vector2(curDashPower, 0f);
    }

    private bool hasPressedDashButton = false;
    private void Dash(InputAction.CallbackContext context)
    {
        if (Block) return;
        hasPressedDashButton =  !hasPressedDashButton;
        if (!hasPressedDashButton || dashBlock || !canDash) return;
        dashBlock = true;
        canDash = false;
        dashTimer.StartTimer();
        if (rg.linearVelocityX > rgDeadZone) curDashPower += dashForce + Addition;
        else if (rg.linearVelocityX < -rgDeadZone) curDashPower -= dashForce + Addition;
    }

    private bool canDash = true;
    private bool dashBlock = false;
    private void OnDashBlock()
    {
        dashBlock = false;
    }
}
