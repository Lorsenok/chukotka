using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [Inject] private void Init(IGameState state)
    {
        gameState = state;
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
        idle.enabled = rg.linearVelocity.x < minSpeedForRunAnim && rg.linearVelocity.x > -minSpeedForRunAnim;
        run.enabled = !idle.enabled;
        if (rg.linearVelocity.x > minSpeedForRunAnim | rg.linearVelocity.x < -minSpeedForRunAnim && flipLock <= 0f)
        {
            spr.flipX = rg.linearVelocity.x < 0f;
            flipLock = fliplockSet;
        }

        flipLock -= Time.deltaTime;
    }
}