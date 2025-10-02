using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IDamageble
{
    [SerializeField] private Rigidbody2D rg;
    public float Power { get; set; } = 20f;
    [SerializeField] private float damage;
    [SerializeField] private float forceAdditionalAngle;
    [SerializeField] private float additionalRotate;

    private void Start()
    {
        Vector3 dir = ProjMath.MoveTowardsAngle(360f - transform.eulerAngles.z - forceAdditionalAngle);
        rg.AddForce(dir.normalized * Power, ForceMode2D.Impulse);
    }

    private bool pin = false;
    private Vector3 pinPos;
    private float pinRotation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool pointed = false;
        
        if (other.gameObject.TryGetComponent(out IDamageble damageble) && !pin)
        {
            pointed = true;
            
            transform.SetParent(other.gameObject.transform);
            /*transform.localScale = 
                new Vector3(
                    transform.localScale.x / other.transform.localScale.x, 
                    transform.localScale.y / other.transform.localScale.y, 
                    transform.localScale.z / other.transform.localScale.z
                );*/
            
            damageble.GetDamage(damage);
        }

        if (other.gameObject.TryGetComponent(out Ground ground) && !pin)
        {
            pointed = true;
        }

        if (pointed)
        {
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
