using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class ControllerAttack : ControllerAddition
{
    [SerializeField] private Rigidbody2D rg;
    [SerializeField, Range(0f, 1f)] private float controlsDeadZone = 0.1f;
    [SerializeField] private float speedMultiplier = 0.2f;
    [SerializeField] private int combo;
    [SerializeField] private int curCombo = 0;
    [SerializeField] private Timer comboTimer;
    [SerializeField] private Timer attackTimer;
    [SerializeField] private Timer perAttackTimer;
    [SerializeField] private Collider2D leftAttack;
    [SerializeField] private Collider2D rightAttack;
    [SerializeField] private GameObject[] spawnOnDamage;
    private float attackDir = 1f;
    
    private InputSystem input;
    private IGameState gameState;
    [Inject] private void Init(IInputControler inputControler, IGameState gameState)
    {
        input = inputControler.GetInputSystem();
        this.gameState = gameState;
    }

    private void OnEnable()
    {
        attackTimer.OnTimerEnd += OnAttackEnd;
        perAttackTimer.OnTimerEnd += OnAttackDelayEnd;
        comboTimer.OnTimerEnd += OnComboEnd;
        
        input.Player.Attack.performed += Attack;
    }

    private void OnDisable()
    {
        attackTimer.OnTimerEnd -= OnAttackEnd;
        perAttackTimer.OnTimerEnd -= OnAttackDelayEnd;
        comboTimer.OnTimerEnd -= OnComboEnd;
        
        input.Player.Attack.performed -= Attack;
    }

    private void Update()
    {
        if (rg.linearVelocityX > controlsDeadZone || rg.linearVelocityX < -controlsDeadZone) attackDir = Mathf.Sign(rg.linearVelocityX);
        SpeedMultiplier = new Vector2(isAttacking ? speedMultiplier : 1f, 1f);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (!canAttack || Block || rg.linearVelocityY != 0f) return;
        if (isCombo)
        {
            curCombo++;
            if (curCombo >= combo + Addition)
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

        isAttacking = true;
        canAttack = false;
        perAttackTimer.StartTimer(); 
        attackTimer.StartTimer();
        leftAttack.enabled = (attackDir < 0f);
        rightAttack.enabled = (attackDir > 0f);

        foreach (GameObject obj in spawnOnDamage)
        {
            Instantiate(obj, transform.position, obj.transform.rotation);
        }
    }

    private bool isAttacking = false;
    private void OnAttackEnd()
    {
        isAttacking = false;
        leftAttack.enabled = (false);
        rightAttack.enabled = (false);
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
}
