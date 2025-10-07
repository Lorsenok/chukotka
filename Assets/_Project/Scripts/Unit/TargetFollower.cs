using System;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{
    public float SpeedMultiplier { get; set; } = 1f;
    
    [SerializeField] protected Transform target;
    [SerializeField] protected Rigidbody2D rg;
    [SerializeField] protected GroundChecker groundChecker;
    
    [Header("Speed")]
    [SerializeField] protected float minDistance;
    [SerializeField] protected float deceleration;
    [SerializeField] protected float speed;
    [SerializeField] protected float acceleration;
    
    [Header("Jump")]
    [SerializeField] protected float jumpForce;
    [SerializeField] protected Timer jumpDelayTimer;
    [SerializeField] protected float minDistanceToJump;
    
    public virtual void OnEnable()
    {
        jumpDelayTimer.OnTimerEnd += OnJumpDelayEnd;
    }

    public virtual void OnDisable()
    {
        jumpDelayTimer.OnTimerEnd -= OnJumpDelayEnd;
    }

    protected bool canJump = true;
    private void OnJumpDelayEnd()
    {
        canJump = true;
    }

    public virtual void Move(float multiplier)
    {
        rg.linearVelocityX += (target.position - transform.position).normalized.x * acceleration * Time.deltaTime * multiplier * SpeedMultiplier;
        rg.linearVelocityX = Mathf.Clamp(rg.linearVelocityX, -speed, speed);
    }

    public virtual void Jump()
    {
        rg.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        canJump = false;
        jumpDelayTimer.StartTimer();
    }

    public virtual void Update()
    {
        rg.linearVelocityX = Mathf.Lerp(rg.linearVelocityX, 0f, deceleration * Time.deltaTime);
        if (Vector2.Distance(transform.position, target.position) > minDistance)
        {
            Move(1f);
        }

        if (canJump && groundChecker.IsTouchingGround && target.position.y - transform.position.y >= minDistanceToJump)
        {
            Jump();
        }
    }
}
