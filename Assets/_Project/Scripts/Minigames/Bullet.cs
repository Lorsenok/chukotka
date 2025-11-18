using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rg;
    [SerializeField] private bool ignoreController;
    public float Power { get; set; } = 20f;
    [SerializeField] private int damage;
    [SerializeField] private float damageBySpeedMultiplier = 1f;
    private float additionalDamage = 0;
    [SerializeField] private float forceAdditionalAngle;
    [SerializeField] private float additionalRotate;
    [SerializeField] private ArrowItem arrowItem;
    
    [SerializeField] private GameObject[] spawnOnDamage;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private VisualAction visualActionOnDamage;
    
    private void Start()
    {
        Vector3 dir = ProjMath.MoveTowardsAngle(360f - transform.eulerAngles.z - forceAdditionalAngle);
        rg.AddForce(dir.normalized * Power, ForceMode2D.Impulse);
        if (arrowItem != null) arrowItem.enabled = false;
    }

    private bool pin = false;
    private Vector3 pinPos;
    private float pinRotation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInChildren<Controller>() & ignoreController || other.isTrigger) return;
        
        if (other.gameObject.TryGetComponent(out DestroyableObject damageble) && !pin)
        {
            damageble.HP -= damage + Mathf.RoundToInt(additionalDamage);
            foreach (GameObject spawn in spawnOnDamage)
            {
                Instantiate(spawn, spawnPoint.position, spawn.transform.rotation);
            }
            VisualActionsHandler.Action(visualActionOnDamage);
            Destroy(gameObject);
        }

        if (other.gameObject.TryGetComponent(out Ground ground) && !pin)
        {
            rg.linearVelocity = Vector2.zero;
            rg.gravityScale = 0f;
            rg.freezeRotation = true;
            
            pin = true;
            pinPos = transform.localPosition;
            pinRotation = transform.localEulerAngles.z;

            if (arrowItem != null) arrowItem.enabled = true;
        }
    }

    private void Update()
    {
        if (pin)
        {
            transform.localPosition = pinPos;
            transform.localEulerAngles = new Vector3(0f, 0f, pinRotation);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f,
                ProjMath.RotateTowardsPosition(rg.linearVelocity.normalized) + additionalRotate);
            
            additionalDamage = Mathf.Max(additionalDamage, Vector2.Distance(rg.linearVelocity, Vector2.zero) * damageBySpeedMultiplier);
        }
    }
}
