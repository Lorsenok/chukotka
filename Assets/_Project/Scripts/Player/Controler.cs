using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Controler : MonoBehaviour
{
    public bool CanMove { get; set; } = true;

    public int AdditionalJumps { get; set; } = 0;
    private int curAdditionalJumps = 0;
    
    public float AdditionalSpeed { get; set; } = 0f;
    public float AdditionalAcceleration { get; set; } = 0f;
    public float AdditionalDeceleration { get; set; } = 0f;
    
    public float AdditionalDashPower { get; set; } = 0f;

    public int AdditionalCombo { get; set; } = 0;

    public bool CanJump { get; set; } = true;

    [Header("Sound")]
    [SerializeField] private AudioSource[] sound;
    [SerializeField] private float soundBlockTimeSet;

    [Header("Movement")]
    [SerializeField] private Rigidbody2D rg;
    
    [SerializeField] private float acceleration;
    [SerializeField] private float speed;
    [SerializeField] private float deceleration;

    [SerializeField, Range(0f, 1f)] private float controlsDeadZone = 0.1f;
    [SerializeField, Range(0f, 1f)] private float decelerationDeadZone = 0.05f;

    [Header("Jump")]
    [SerializeField] private int jumps = 1;
    [SerializeField] private float jumpForce;
    [SerializeField] private Timer jumpBlockTimer;

    [Header("Dash")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashForceExpireSpeed;
    [SerializeField] private Timer dashTimer;
    private float curDashPower = 0f;

    [Header("Attacking")]
    [SerializeField] private int combo;
    [SerializeField] private int curCombo = 0;
    [SerializeField] private Timer comboTimer;
    [SerializeField] private Timer attackTimer;
    [SerializeField] private Timer perAttackTimer;
    [SerializeField] private GameObject leftAttack;
    [SerializeField] private GameObject rightAttack;
    private float attackDir = 1f;
    
    [Header("Bow")]
    [SerializeField] private Gun gun;
    [SerializeField] private float shootPower = 20f;
    [SerializeField] private float timeToLoadSet;
    [SerializeField] private float startTimeToLoad;
    private float curLoadTime = 0f;
    [SerializeField] private Timer perLoadTimer;
    [SerializeField] private float loadSpeedMultiplier;
    
    private InputSystem input;
    private IGameState gameState;
    [Inject] private void Init(IInputControler inputControler, IGameState gameState)
    {
        input = inputControler.GetInputSystem();
        this.gameState = gameState;
    }

    private void OnEnable()
    {
        input.UI.Click.performed += Shoot;
        perLoadTimer.OnTimerEnd += OnLoadEnd;
        
        input.Player.Attack.performed += Attack;
        attackTimer.OnTimerEnd += OnAttackEnd;
        perAttackTimer.OnTimerEnd += OnAttackDelayEnd;
        comboTimer.OnTimerEnd += OnComboEnd;
        
        input.Player.Dash.performed += Dash;
        dashTimer.OnTimerEnd += OnDashBlock;
        
        input.Player.Jump.performed += Jump;
        jumpBlockTimer.OnTimerEnd += OnJumpBlock;
    }

    private void OnDisable()
    {
        input.UI.Click.performed -= Shoot;
        perLoadTimer.OnTimerEnd -= OnLoadEnd;
        
        input.Player.Attack.performed -= Attack;
        attackTimer.OnTimerEnd -= OnAttackEnd;
        perAttackTimer.OnTimerEnd -= OnAttackDelayEnd;
        comboTimer.OnTimerEnd -= OnComboEnd;
        
        input.Player.Dash.performed -= Dash;
        dashTimer.OnTimerEnd -= OnDashBlock;
        
        input.Player.Jump.performed -= Jump;
        jumpBlockTimer.OnTimerEnd -= OnJumpBlock;
    }

    private int curSound = 0;
    private void PlaySound()
    {
        curSound++;
        if (curSound == sound.Length) curSound = 0;

        for (int i = 0; i < sound.Length; i++)
        {
            if (i == curSound) sound[i].Play();
        }
    }

    private void StopSound()
    {
        foreach (var sound in sound)
        {
            sound.Stop();
        }
    }
    
    private Vector2 curSpeed = Vector2.zero;
    private Vector2 controls = Vector2.zero;
    private void Move()
    {
        controls = input.Player.Move.ReadValue<Vector2>();

        curSpeed += Vector2.Lerp(Vector2.zero, controls, (acceleration + AdditionalAcceleration) * Time.deltaTime);

        if (controls.x < controlsDeadZone && controls.x > -controlsDeadZone)
        {
            float x = -curSpeed.x;
            if (x > 0f) x = 1f;
            if (x < 0f) x = -1f;
            curSpeed += Vector2.Lerp(Vector2.zero, new Vector2(x, 0f), Time.deltaTime * (deceleration + AdditionalDeceleration));

            if (curSpeed.x < decelerationDeadZone && curSpeed.x > -decelerationDeadZone)
            {
                curSpeed.x = 0f;
            }
        }
        
        curSpeed.x = Mathf.Clamp(curSpeed.x, -(speed + AdditionalSpeed), speed + AdditionalSpeed);
        rg.linearVelocityX = curSpeed.x + curDashPower;
    }

    private void Update()
    {
        if (gameState.GetCurrectState() != GameState.Game)
        {
            rg.linearVelocityX = 0f;
            return;
        }

        if (!sound[curSound].isPlaying && curSpeed.x != 0f && CanJump)
        {
            PlaySound();
        }

        if (CanMove) Move();
        else
        {
            if (sound != null) StopSound();
            rg.linearVelocityX = 0f;
        }

        if (CanJump) canDash = true;
        curDashPower = Mathf.Lerp(curDashPower, 0f, Time.deltaTime * dashForceExpireSpeed);

        if (curSpeed.x != 0f) attackDir = Mathf.Sign(curSpeed.x);

        if (isHolding)
        {
            curLoadTime += Time.deltaTime;
            curLoadTime = Mathf.Clamp(curLoadTime, 0f, timeToLoadSet);
        }
    }

    private bool hasPressedJumpButton = false;
    private void Jump(InputAction.CallbackContext context)
    {
        hasPressedJumpButton = !hasPressedJumpButton;
        if (!CanJump & curAdditionalJumps <= 0 || !hasPressedJumpButton || jumpBlock) return;
        jumpBlockTimer.StartTimer();
        jumpBlock = true;
        curAdditionalJumps--;
        if (CanJump) curAdditionalJumps = jumps + AdditionalJumps;
        rg.linearVelocityY = jumpForce;
    }

    private bool jumpBlock = false;
    private void OnJumpBlock()
    {
        jumpBlock = false;
    }

    private bool hasPressedDashButton = false;
    private void Dash(InputAction.CallbackContext context)
    {
        hasPressedDashButton =  !hasPressedDashButton;
        if (!hasPressedDashButton || dashBlock || !canDash) return;
        dashBlock = true;
        canDash = false;
        dashTimer.StartTimer();
        if (rg.linearVelocityX > controlsDeadZone) curDashPower += dashForce + AdditionalDashPower;
        else if (rg.linearVelocityX < -controlsDeadZone) curDashPower -= dashForce;
    }

    private bool canDash = true;
    private bool dashBlock = false;
    private void OnDashBlock()
    {
        dashBlock = false;
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (!canAttack || isLoading) return;
        if (isCombo)
        {
            curCombo++;
            if (curCombo >= combo + AdditionalCombo)
            {
                curCombo--;
                return;
            }
        }
        else
        {
            isCombo = true;
            comboTimer.StartTimer();
        }

        canAttack = false;
        perAttackTimer.StartTimer(); 
        attackTimer.StartTimer();
        leftAttack.SetActive(attackDir < 0f);
        rightAttack.SetActive(attackDir > 0f);
    }

    private void OnAttackEnd()
    {
        leftAttack.SetActive(false);
        rightAttack.SetActive(false);
    }

    private bool canAttack = false;
    private void OnAttackDelayEnd()
    {
        canAttack = true;
    }

    private bool isCombo = false;
    private void OnComboEnd()
    {
        isCombo = false;
        curCombo = 0;
    }

    private bool holdedWhileLoad = false;
    private bool isHolding = false;
    private void Shoot(InputAction.CallbackContext context)
    {
        holdedWhileLoad = !holdedWhileLoad;
        if (isLoading) holdedWhileLoad = false;
        isHolding = !isHolding;
        if (holdedWhileLoad != isHolding)
        {
            isHolding = holdedWhileLoad;
            return;
        }
        if (!isHolding && !isLoading)
        {
            gun.Shoot(shootPower * (1 / timeToLoadSet * (curLoadTime + startTimeToLoad)));
            curLoadTime = 0f;
            perLoadTimer.StartTimer();
            isLoading = true;
        }
    }

    private bool isLoading = false;
    private void OnLoadEnd()
    {
        isLoading = false;
    }
}