using UnityEngine;

public class Animal : TargetFollower
{
    [Header("Animal Settings")]
    [SerializeField] private DestroyableObject destroyableObject;
    [SerializeField] private GameObject[] dropPrefab;

    [Header("Escape")]
    [SerializeField] private float escapeSpeedMultiplier = 1f;
    [SerializeField] private int hpWhenEscaping;
    [SerializeField] private Timer timeToEscape;
    [SerializeField] private GameObject[] spawnAfterEscapePrefabs;

    private bool isEscaping = false;
    private bool aggro = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Controller>())
            aggro = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (destroyableObject != null) destroyableObject.OnDie += Drop;
        if (timeToEscape != null) timeToEscape.OnTimerEnd += OnEscape;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (destroyableObject != null) destroyableObject.OnDie -= Drop;
        if (timeToEscape != null) timeToEscape.OnTimerEnd -= OnEscape;
    }

    private void Start()
    {
        if (target == null)
            target = FindFirstObjectByType<Controller>().transform;
    }

    protected override void Update()
    {
        if (destroyableObject != null && destroyableObject.HP <= 0) return;
        if (!aggro) return;

        if (destroyableObject.HP <= hpWhenEscaping && !isEscaping)
        {
            isEscaping = true;
            if (timeToEscape != null) timeToEscape.StartTimer();
        }

        if (isEscaping)
            rg.linearVelocity = new Vector2(-escapeSpeedMultiplier * speed, rg.linearVelocity.y);
        else
            base.Update();
    }

    protected override void Jump(float jumpHeight)
    {
        base.Jump(jumpHeight);
    }

    private void OnEscape()
    {
        if (!isEscaping) return;
        foreach (var obj in spawnAfterEscapePrefabs)
            Instantiate(obj, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void Drop()
    {
        if (dropPrefab.Length == 0) return;
        int rand = Random.Range(0, dropPrefab.Length);
        Instantiate(dropPrefab[rand], transform.position, dropPrefab[rand].transform.rotation);
    }
}