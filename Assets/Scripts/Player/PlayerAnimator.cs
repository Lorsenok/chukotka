using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private CustomAnimator idle;
    [SerializeField] private CustomAnimator run;
    [SerializeField] private CustomAnimator talk;

    [SerializeField] private Rigidbody2D rg;
    [SerializeField] private SpriteRenderer spr;

    [SerializeField] private float minSpeedForRunAnim = 0.1f;

    [SerializeField] private float fliplockSet;
    private float flipLock = 0f;

    [SerializeField] private Timer gameLayerSwitchAnimationDelay;

    private IGameState gameState;
    private InputSystem inputSystem;
    [Inject] private void Init(IGameState state, IInputControler input)
    {
        gameState = state;
        inputSystem = input.GetInputSystem();
    }

    private bool isLayerSwitching = false;

    private void OnEnable()
    {
        gameLayerSwitchAnimationDelay.OnTimerEnd += OnLayerSwitchDelayEnd;
        inputSystem.Player.SwitchLayerDown.performed += OnLayerSwitch;
        inputSystem.Player.SwitchLayerUp.performed += OnLayerSwitch;
    }

    private void OnDisable()
    {
        gameLayerSwitchAnimationDelay.OnTimerEnd -= OnLayerSwitchDelayEnd;
        inputSystem.Player.SwitchLayerDown.performed -= OnLayerSwitch;
        inputSystem.Player.SwitchLayerUp.performed -= OnLayerSwitch;
    }

    private void OnLayerSwitchDelayEnd()
    {
        isLayerSwitching = false;
    }

    private void OnLayerSwitch(InputAction.CallbackContext context)
    {
        isLayerSwitching = true;
        gameLayerSwitchAnimationDelay.StartTimer();
    }

    private void Update()
    {
        if (gameState.GetCurrectState() == GameState.Cutscene)
        {
            talk.enabled = true;
            run.enabled = false;
            idle.enabled = false;
            return;
        }
        talk.enabled = false;
        idle.enabled = rg.linearVelocity.x < minSpeedForRunAnim && rg.linearVelocity.x > -minSpeedForRunAnim && !isLayerSwitching;
        run.enabled = !idle.enabled;
        if (rg.linearVelocity.x > minSpeedForRunAnim | rg.linearVelocity.x < -minSpeedForRunAnim && flipLock <= 0f)
        {
            spr.flipX = rg.linearVelocity.x < 0f;
            flipLock = fliplockSet;
        }

        flipLock -= Time.deltaTime;
    }
}