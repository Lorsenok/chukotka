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
        GameLayersControler.OnLayerSwitch += OnLayerSwitch;
    }

    private void OnDisable()
    {
        GameLayersControler.OnLayerSwitch -= OnLayerSwitch;
    }

    private void OnLayerSwitch()
    {
        isLayerSwitching = true;
    }

    private void Update()
    {
        bool isMoving = rg.linearVelocity.x > minSpeedForRunAnim | rg.linearVelocity.x < -minSpeedForRunAnim
                        || rg.linearVelocity.y > minSpeedForRunAnim | rg.linearVelocity.y < -minSpeedForRunAnim;
        if (gameState.GetCurrentState() == GameState.Cutscene)
        {
            talk.enabled = true;
            run.enabled = false;
            idle.enabled = false;
            return;
        }
        talk.enabled = false;
        idle.enabled = !isMoving && !isLayerSwitching;
        run.enabled = !idle.enabled;
        if (isMoving && flipLock <= 0f)
        {
            spr.flipX = rg.linearVelocity.x < 0f;
            flipLock = fliplockSet;
        }

        flipLock -= Time.deltaTime;
    }
}