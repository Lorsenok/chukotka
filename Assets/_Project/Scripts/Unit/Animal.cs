using System;
using UnityEngine;
using Random = System.Random;

public class Animal : TargetFollower
{
    [Header("Animal")]
    [SerializeField] private DestroyableObject destroyableObject;
    [SerializeField] private GameObject[] dropPrefab;

    [Header("Escaping")] [SerializeField] private float escapeSpeedMultiplier = 1f;
    [SerializeField] private int hpWhenEscaping;
    [SerializeField] private Timer timeToEscape;
    [SerializeField] private GameObject[] spawnAfterEscapePrefabs;

    private bool isEscaping = false;
    
    private bool agr = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Controller>())
        {
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

    private void Start()
    {
        target = FindObjectsByType<Controller>(FindObjectsSortMode.None)[0].transform;
    }

    public override void Update()
    {
        if (!agr) return;
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
            Instantiate(dropPrefab[rand], transform.position, dropPrefab[rand].transform.rotation);
        }
    }
}
