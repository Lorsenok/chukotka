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

    [Header("Animation")]
    [SerializeField] protected CustomAnimator idle;
    [SerializeField] protected CustomAnimator move;
    [SerializeField] protected SpriteRenderer spr;
    [SerializeField] protected float minVelocityToMove;

    protected bool isPassive = false;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetPassive(bool value)
    {
        isPassive = value;
    }

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

    public void Stop()
    {
        rg.linearVelocity = new Vector2(0f, rg.linearVelocity.y);
    }

    public void Move(float multiplier)
    {
        if (target == null) return;
        rg.linearVelocity = new Vector2(
            Mathf.Clamp(rg.linearVelocity.x + (target.position - transform.position).normalized.x * acceleration * Time.deltaTime * multiplier * SpeedMultiplier, -speed, speed),
            rg.linearVelocity.y
        );
    }

    public virtual void Jump()
    {
        rg.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        canJump = false;
        jumpDelayTimer.StartTimer();
    }

    private float ignoreAnimationsTime = 0f;
    private CustomAnimator lastAnimator;
    public void PullOtherAnimation(CustomAnimator animator, float time)
    {
        lastAnimator = animator;
        animator.enabled = true;
        ignoreAnimationsTime = time;
    }

    public virtual void Update()
    {
        if (isPassive)
        {
            rg.linearVelocity = new Vector2(0f, rg.linearVelocity.y);
            if (idle != null) idle.enabled = true;
            if (move != null) move.enabled = false;
            return;
        }

        rg.linearVelocity = new Vector2(Mathf.Lerp(rg.linearVelocity.x, 0f, deceleration * Time.deltaTime), rg.linearVelocity.y);

        if (target != null && Vector2.Distance(transform.position, target.position) > minDistance)
            Move(1f);

        if (target != null && canJump && groundChecker.IsTouchingGround && target.position.y - transform.position.y >= minDistanceToJump)
            Jump();

        if (ignoreAnimationsTime > 0f)
        {
            ignoreAnimationsTime -= Time.deltaTime;
            return;
        }

        if (lastAnimator != null) lastAnimator.enabled = false;
        bool isMoving = rg.linearVelocity.x > minVelocityToMove || rg.linearVelocity.x < -minVelocityToMove;
        if (idle != null) idle.enabled = !isMoving;
        if (move != null) move.enabled = isMoving;
        if (spr != null && isMoving) spr.flipX = rg.linearVelocity.x < 0f;
    }
}