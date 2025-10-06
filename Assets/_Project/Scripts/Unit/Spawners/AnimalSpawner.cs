using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalSpawner : Spawner
{
    private Vector3 startPosition;
    [Header("Animal Spawner")]
    [SerializeField] private float range;
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
        transform.position =
            new Vector3(startPosition.x + Random.Range(-range, range), startPosition.y, startPosition.z);
    }

    public void Start()
    {
        startPosition = transform.position;
    }
}
