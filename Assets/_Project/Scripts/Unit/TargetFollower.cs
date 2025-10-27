using UnityEngine;

public class TargetFollower : MonoBehaviour
{
    [Header("Target & Rigidbody")]
    [SerializeField] protected Transform target;
    [SerializeField] protected Rigidbody2D rg;
    [SerializeField] protected GroundChecker groundChecker;

    [Header("Movement Settings")]
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float acceleration = 20f;
    [SerializeField] protected float deceleration = 15f;
    [SerializeField] protected float minDistance = 0.1f;

    [Header("Jump Settings")]
    [SerializeField] protected float minDistanceToJump = 1f;
    [SerializeField] protected float jumpForce = 10f;
    [SerializeField] protected Timer jumpDelayTimer;

    [Header("Animation")]
    [SerializeField] protected CustomAnimator idle;
    [SerializeField] protected CustomAnimator move;
    [SerializeField] protected SpriteRenderer spr;
    [SerializeField] protected float minVelocityToMove = 0.1f;

    public float SpeedMultiplier { get; set; } = 1f;
    protected bool canJump = true;

    public void Stop()
    {
        if (rg != null)
            rg.linearVelocity = new Vector2(0f, rg.linearVelocity.y); // останавливаем только горизонтальное движение
    }

    protected virtual void OnEnable()
    {
        if (jumpDelayTimer != null)
            jumpDelayTimer.OnTimerEnd += OnJumpDelayEnd;
    }

    protected virtual void OnDisable()
    {
        if (jumpDelayTimer != null)
            jumpDelayTimer.OnTimerEnd -= OnJumpDelayEnd;
    }

    private void OnJumpDelayEnd()
    {
        canJump = true;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    protected virtual void Update()
    {
        if (target == null) return;

        HandleMovement();
        HandleJump();
        HandleAnimation();
    }

    private void HandleMovement()
    {
        float directionX = target.position.x - transform.position.x;

        float desiredVelocityX = 0f;
        if (Mathf.Abs(directionX) > minDistance)
            desiredVelocityX = Mathf.Sign(directionX) * speed * SpeedMultiplier;

        rg.linearVelocity = new Vector2(
            Mathf.MoveTowards(rg.linearVelocity.x, desiredVelocityX, acceleration * Time.deltaTime),
            rg.linearVelocity.y
        );
    }

    private void HandleJump()
    {
        if (!canJump || target == null) return;

        float distanceY = target.position.y - transform.position.y;

        if (distanceY > minDistanceToJump && groundChecker.IsTouchingGround)
        {
            Jump(distanceY);
        }
    }

    protected virtual void Jump(float jumpHeight)
    {
        // Сброс вертикальной скорости перед прыжком
        rg.linearVelocity = new Vector2(rg.linearVelocity.x, 0f);

        // Расчет силы прыжка для достижения цели
        float jumpVelocity = Mathf.Sqrt(2f * Mathf.Abs(Physics2D.gravity.y) * jumpHeight);
        rg.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);

        canJump = false;
        if (jumpDelayTimer != null)
            jumpDelayTimer.StartTimer();
    }

    private void HandleAnimation()
    {
        bool isMoving = Mathf.Abs(rg.linearVelocity.x) > minVelocityToMove;

        if (idle != null) idle.enabled = !isMoving;
        if (move != null) move.enabled = isMoving;

        if (spr != null && isMoving)
            spr.flipX = rg.linearVelocity.x < 0f;
    }
}