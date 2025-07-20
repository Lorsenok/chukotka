using UnityEngine;

public class Bullet : MonoBehaviour, IDamageble
{
    [SerializeField] private Rigidbody2D rg;
    [SerializeField] private float power;
    [SerializeField] private float damage;
    [SerializeField] private float forceAdditionalAngle;
    [SerializeField] private float additionalRotate;

    private void Start()
    {
        Vector3 dir = ProjMath.MoveTowardsAngle(360f - transform.eulerAngles.z - forceAdditionalAngle);
        rg.AddForce(dir.normalized * power, ForceMode2D.Impulse);
    }

    private bool pin = false;
    private Vector3 pinPos;
    private float pinRotation;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageble damageble) && !pin)
        {
            damageble.GetDamage(damage);
            transform.SetParent(collision.gameObject.transform);
            rg.linearVelocity = Vector2.zero;
            rg.gravityScale = 0f;
            rg.freezeRotation = true;
            pin = true;
            pinPos = transform.localPosition;
            pinRotation = transform.localEulerAngles.z;
        }
    }

    private void OnEnable()
    {
        Minigame.OnMinigameEnd += Die;
    }

    private void OnDestroy()
    {
        Minigame.OnMinigameEnd -= Die;
    }

    public void GetDamage(float damage)
    {
        if (!pin) Destroy(gameObject);
    }

    public void Die()
    {
        GetDamage(0f);
    }

    private void Update()
    {
        if (pin)
        {
            transform.localPosition = pinPos;
            transform.localEulerAngles = new Vector3(0f, 0f, pinRotation);
        }
        else transform.eulerAngles = new Vector3(0f, 0f, ProjMath.RotateTowardsPosition(rg.linearVelocity.normalized) + additionalRotate);
    }
}
