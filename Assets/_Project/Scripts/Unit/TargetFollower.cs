using System;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody2D rg;
    [SerializeField] private GroundChecker groundChecker;
    
    [Header("Speed")]
    [SerializeField] private float minDistance;
    [SerializeField] private float deceleration;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    
    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private Timer jumpDelayTimer;
    [SerializeField] private float minDistanceToJump;
    
    private void OnEnable()
    {
        jumpDelayTimer.OnTimerEnd += OnJumpDelayEnd;
    }

    private void OnDisable()
    {
        jumpDelayTimer.OnTimerEnd -= OnJumpDelayEnd;
    }

    private bool canJump = true;
    private void OnJumpDelayEnd()
    {
        canJump = true;
    }

    private void Update()
    {
        rg.linearVelocityX = Mathf.Lerp(rg.linearVelocityX, 0f, deceleration * Time.deltaTime);
        if (Vector2.Distance(transform.position, target.position) > minDistance)
        {
            rg.linearVelocityX += (target.position - transform.position).normalized.x * acceleration * Time.deltaTime;
            rg.linearVelocityX = Mathf.Clamp(rg.linearVelocityX, -speed, speed);
        }

        if (canJump && groundChecker.IsTouchingGround && target.position.y - transform.position.y >= minDistanceToJump)
        {
            rg.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
            jumpDelayTimer.StartTimer();
        }
    }
}
