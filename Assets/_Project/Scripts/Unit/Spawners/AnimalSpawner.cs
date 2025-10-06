using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalSpawner : Spawner
{
    private Vector3 startPosition;
    [Header("Animal Spawner")]
    
    [SerializeField] private bool lockToTarget;
    [SerializeField] private Transform target;
    
    [SerializeField] private float maxRange;
    [SerializeField] private float minRange;
    [SerializeField] private Timer changePositionTimer;

    public override void OnEnable()
    {
        base.OnEnable();
        changePositionTimer.OnTimerEnd += OnPositionChange;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        changePositionTimer.OnTimerEnd -= OnPositionChange;
    }

    private void OnPositionChange()
    {
        int maxIterations = 100;
        float curRange = 0f;
        while (curRange < minRange && curRange > -minRange)
        {
            curRange = Random.Range(-maxRange, maxRange);
            maxIterations--;
            if (maxIterations <= 0)
            {
                Debug.LogError("Too many tries to set position in range!");
                return;
            }
        }
        transform.position =
            new Vector3(startPosition.x + curRange, startPosition.y, startPosition.z);
    }

    public void Start()
    {
        startPosition = transform.position;
    }

    public void Update()
    {
        if (!lockToTarget) return;
        startPosition = target.position;
    }
}
