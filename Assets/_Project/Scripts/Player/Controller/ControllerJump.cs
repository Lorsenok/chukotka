using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class ControllerJump : ControllerAddition
{
    private int curAdditionalJumps = 0;

    [SerializeField] private Rigidbody2D rg;
    [SerializeField] private GroundChecker groundChecker;
    
    [SerializeField] private int jumps = 1;
    [SerializeField] private float jumpForce;
    [SerializeField] private Timer jumpBlockTimer;

    [Header("Animation")]
    [SerializeField] private CustomAnimatorController animController;
    [SerializeField] private string jumpAnim;
    [SerializeField] private float jumpAnimTime;
    
    private InputSystem input;
    private IGameState gameState;
    [Inject] private void Init(IInputControler inputControler, IGameState gameState)
    {
        input = inputControler.GetInputSystem();
        this.gameState = gameState;
    }
    
    private void OnEnable()
    {
        input.Player.Jump.performed += Jump;
        jumpBlockTimer.OnTimerEnd += OnJumpBlock;
    }

    private void OnDisable()
    {
        input.Player.Jump.performed -= Jump;
        jumpBlockTimer.OnTimerEnd -= OnJumpBlock;
    }

    private bool hasJumped = false;
    private bool hasPressedJumpButton = false;
    private void Jump(InputAction.CallbackContext context)
    {
        if (Block) return;
        hasPressedJumpButton = !hasPressedJumpButton;
        if (!groundChecker.IsTouchingGround & curAdditionalJumps <= 0 || !hasPressedJumpButton || jumpBlock) return;
        jumpBlockTimer.StartTimer();
        jumpBlock = true;
        curAdditionalJumps--;
        if (groundChecker.IsTouchingGround) curAdditionalJumps = jumps + Addition;
        rg.linearVelocityY = jumpForce;
        
        animController.PullAnimation(jumpAnim, jumpAnimTime);
        animController.ResetAnimation();
        hasJumped = true;
    }

    public void Update()
    {
        if (!hasJumped) return;
        hasJumped = !(groundChecker.IsTouchingGround & !jumpBlock);
        if (!hasJumped)
        {
            animController.ResetController();
            return;
        }
        animController.PullAnimation(jumpAnim, jumpAnimTime);
    }
    
    private bool jumpBlock = false;
    private void OnJumpBlock()
    {
        jumpBlock = false;
    }
}
