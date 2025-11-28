using System;
using UnityEngine;
using Random = System.Random;

public class Animal : TargetFollower
{
    [Header("Animal")] [SerializeField] private Transform targetWhileNotAgr;
    [SerializeField] private DestroyableObject destroyableObject;
    [SerializeField] private GameObject[] dropPrefab;
    [SerializeField] private GameObject[] spawnAfterDying;
    [SerializeField] private Transform lootSpawnpoint;
    [SerializeField] private bool agrByTrigger = true;
    
    [Header("Escaping")][SerializeField] private float escapeSpeedMultiplier = 1f;
    [SerializeField] private int hpWhenEscaping;
    [SerializeField] private Timer timeToEscape;
    [SerializeField] private GameObject[] spawnAfterEscapePrefabs;

    [Header("Custom Animator")]
    [SerializeField] private float minSpeedX = 0.25f;
    [SerializeField] private CustomAnimatorController animController;
    [SerializeField] private string idleAnim;
    [SerializeField] private float idleAnimTime;
    [SerializeField] private string walkAnim;
    [SerializeField] private float walkAnimTime;
    [SerializeField] private string jumpAnim;
    [SerializeField] private float jumpAnimTime;

    private bool isEscaping = false;

    private bool agr = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Controller>() && agrByTrigger)
        {
            target = other.transform;
            agr = true;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        destroyableObject.OnDie += Drop;
        timeToEscape.OnTimerEnd += OnEscape;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        destroyableObject.OnDie -= Drop;
        timeToEscape.OnTimerEnd -= OnEscape;
    }

    public override void Update()
    {
        if (destroyableObject.HP <= 0) return;
        bool isMoving = rg.linearVelocityX > minVelocityToMove || rg.linearVelocityX < -minVelocityToMove;
        if (!agr)
        {
            if (targetWhileNotAgr)
            {
                target = targetWhileNotAgr;
                base.Update();
            }
            return;
        }
        if (destroyableObject.HP <= hpWhenEscaping && !isEscaping)
        {
            isEscaping = true;
            timeToEscape.StartTimer();
        }
        if (isEscaping)
        {
            Move(-escapeSpeedMultiplier);
            if (groundChecker.IsTouchingGround && canJump) Jump();
        }
        else base.Update();

        if (isMoving && animController != null && Mathf.Abs(rg.linearVelocityX) > minSpeedX && Vector2.Distance(transform.position, target.position) > minDistance)
        {
            animController.PullAnimation(walkAnim, walkAnimTime);
        }
        else if (animController != null)
        {
            animController.PullAnimation(idleAnim, idleAnimTime);
        }
    }

    public override void Jump()
    {
        base.Jump();
        if (animController != null) animController.PullAnimation(jumpAnim, jumpAnimTime);
    }

    private void OnEscape()
    {
        if (!isEscaping) return;
        foreach (GameObject obj in spawnAfterEscapePrefabs)
            Instantiate(obj, transform.position, obj.transform.rotation);
        Destroy(gameObject);
    }

    private void Drop()
    {
        if (dropPrefab.Length != 0)
        {
            int rand = UnityEngine.Random.Range(0, dropPrefab.Length);
            if (lootSpawnpoint != null)
            {
                Instantiate(dropPrefab[rand], lootSpawnpoint.position, dropPrefab[rand].transform.rotation);
            }
            else
            {
                Instantiate(dropPrefab[rand], transform.position, dropPrefab[rand].transform.rotation);
            }
        }

        foreach (GameObject obj in spawnAfterDying)
        {
            Instantiate(obj, transform.position, obj.transform.rotation);
        }
    }
}