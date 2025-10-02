using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class ControllerBow : ControllerAddition
{
    [SerializeField] private ControllerAddition[] controllersBlock;
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
        input.Player.Shoot.performed += Shoot;
        perLoadTimer.OnTimerEnd += OnLoadEnd;
    }

    private void OnDisable()
    {
        input.Player.Shoot.performed -= Shoot;
        perLoadTimer.OnTimerEnd -= OnLoadEnd;
    }

    private void Update()
    {
        SpeedMultiplier = new Vector2(isHolding ? loadSpeedMultiplier : 1.0f, 1f);

        foreach (var controller in controllersBlock)
        {
            controller.Block = isHolding;
        }
        
        if (isHolding)
        {
            curLoadTime += Time.deltaTime;
            curLoadTime = Mathf.Clamp(curLoadTime, 0f, timeToLoadSet);
        }
    }

    private bool isHoldingWhileLoad = false;
    private bool isHolding = false;
    private void Shoot(InputAction.CallbackContext context)
    {
        isHoldingWhileLoad = !isHoldingWhileLoad;
        if (isLoading) return;
        isHolding = !isHolding;
        if (!isHoldingWhileLoad && isHolding)
        {
            isHoldingWhileLoad = false;
            isHolding = false;
            return;
        }
        if (!isHolding)
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
