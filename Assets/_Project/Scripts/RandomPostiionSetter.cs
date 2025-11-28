using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomPostiionSetter : MonoBehaviour
{
    [SerializeField] private bool lockToTarget;
    [SerializeField] private Transform target;
    
    [SerializeField] private Timer changePositionDelay;
    [SerializeField] private float minRange;
    [SerializeField] private float maxRange;

    private Vector3 startPosition;
    
    private void OnEnable()
    {
        changePositionDelay.OnTimerEnd += OnChangePosition;
    }

    private void OnDisable()
    {
        changePositionDelay.OnTimerEnd -= OnChangePosition;
    }

    private void OnChangePosition()
    {
        int maxIterations = 100;
        float curRange = 0f;
        while (curRange < minRange & curRange > -minRange ||
               startPosition.x + curRange > BorderController.MaxDistance |
               startPosition.x + curRange < BorderController.MinDistance)
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
        if (!target) target = FindObjectsByType<Controller>(FindObjectsSortMode.None)[0].transform;
        startPosition = target.position;
    }
}
