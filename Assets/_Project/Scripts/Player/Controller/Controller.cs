using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Controller : MonoBehaviour
{
    public bool CanMove { get; set; } = true;
    
    public float AdditionalSpeed { get; set; } = 0f;
    public float AdditionalAcceleration { get; set; } = 0f;
    public float AdditionalDeceleration { get; set; } = 0f;

    [SerializeField] private ControllerAddition[] controllerAdditions;

    [Header("Sound")]
    [SerializeField] private AudioSource[] sound;
    [SerializeField] private float soundBlockTimeSet;
    [SerializeField] private Timer soundDelayTimer;

    [Header("Movement")]
    [SerializeField] private Rigidbody2D rg;
    
    [SerializeField] private float acceleration;
    [SerializeField] private float speed;
    [SerializeField] private float deceleration;

    [SerializeField, Range(0f, 1f)] private float controlsDeadZone = 0.1f;
    [SerializeField, Range(0f, 1f)] private float decelerationDeadZone = 0.05f;
    
    private InputSystem input;
    private IGameState gameState;
    [Inject] private void Init(IInputControler inputControler, IGameState gameState)
    {
        input = inputControler.GetInputSystem();
        this.gameState = gameState;
    }

    private void OnEnable()
    {
        soundDelayTimer.OnTimerEnd += PlaySound;
    }

    private void OnDisable()
    {
        soundDelayTimer.OnTimerEnd -= PlaySound;
    }

    private int curSound = 0;
    private void PlaySound()
    {
        if (sound == null) return;
        if (curSpeed.x == 0f || rg.linearVelocityY != 0f)
        {
            StopSound();
            return;
        }
        
        curSound++;
        if (curSound == sound.Length) curSound = 0;

        for (int i = 0; i < sound.Length; i++)
        {
            if (i == curSound) sound[i].Play();
        }
    }

    private void StopSound()
    {
        foreach (var s in sound)
        {
            s.Stop();
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

        Vector2 additive = Vector2.zero;
        Vector2 additiveM = new Vector2(1.0f, 1.0f);
        foreach (ControllerAddition ca in controllerAdditions)
        {
            additive += ca.AdditionalSpeed;
            additiveM *= ca.SpeedMultiplier;
        }

        soundDelayTimer.SpeedMultiplier = additiveM.x;
        curSpeed.x = Mathf.Clamp(curSpeed.x, -(speed + AdditionalSpeed), speed + AdditionalSpeed);
        rg.linearVelocityX = curSpeed.x * additiveM.x + additive.x;
    }

    private void Update()
    {
        if (gameState.GetCurrentState() != GameState.Game)
        {
            rg.linearVelocityX = 0f;
            return;
        }

        if (CanMove) Move();
        else
        {
            if (sound != null) StopSound();
            rg.linearVelocityX = 0f;
        }

    }
}