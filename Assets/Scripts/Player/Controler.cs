using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Controler : MonoBehaviour
{
    public bool CanMove { get; set; } = true;

    public float AdditionalSpeed { get; set; } = 0f;
    public float AdditionalAcceleration { get; set; } = 0f;
    public float AdditionalDeceleration { get; set; } = 0f;

    public bool CanJump { get; set; } = false;

    [SerializeField] private AudioSource sound;
    [SerializeField] private float soundBlockTimeSet;

    private float curSoundBlockTime = 0f;

    [SerializeField] private Rigidbody2D rg;

    [SerializeField] private float acceleration;
    [SerializeField] private float speed;
    [SerializeField] private float decceleration;

    [SerializeField, Range(0f, 1f)] private float controlsDeadZone = 0.1f;
    [SerializeField, Range(0f, 1f)] private float deceleraitionDeadZone = 0.05f;

    [SerializeField] private float jumpForse;

    private float curSpeedX = 0f;
    private float controlsX = 0f;

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
    }

    private void OnDisable()
    {
        input.Player.Jump.performed -= Jump;
    }

    private void Move()
    {
        controlsX = input.Player.Move.ReadValue<Vector2>().x;

        curSpeedX += Mathf.Lerp(0f, controlsX, (acceleration + AdditionalAcceleration) * Time.deltaTime);

        if (controlsX < controlsDeadZone && controlsX > -controlsDeadZone)
        {
            float x = -curSpeedX;
            if (x > 0f) x = 1f;
            if (x < 0f) x = -1f;
            curSpeedX += Mathf.Lerp(0f, x, Time.deltaTime * (decceleration + AdditionalDeceleration));

            if (curSpeedX < deceleraitionDeadZone && curSpeedX > -deceleraitionDeadZone)
            {
                curSpeedX = 0f;
            }
        }

        curSpeedX = Mathf.Clamp(curSpeedX, -(speed + AdditionalSpeed), speed + AdditionalSpeed);
        rg.linearVelocity = new Vector2(curSpeedX, rg.linearVelocityY);
    }

    private void Update()
    {
        if (gameState.GetCurrectState() != GameState.Game) return;

        if (rg.linearVelocity.x > 0.1f | rg.linearVelocity.x < -0.1f && sound != null)
        {
            if (!sound.isPlaying && curSoundBlockTime <= 0)
            {
                sound.Play();
                curSoundBlockTime = soundBlockTimeSet;
            }
        }
        else if (sound != null) sound.Stop();

        if (CanMove) Move();
        else if (sound != null) sound.Stop();
    }

    private bool hasPressedJumpButton = false;
    private void Jump(InputAction.CallbackContext context)
    {
        hasPressedJumpButton = !hasPressedJumpButton;
        if (!CanJump || !hasPressedJumpButton) return;
        rg.AddForce(Vector2.up * jumpForse, ForceMode2D.Impulse);
    }
}