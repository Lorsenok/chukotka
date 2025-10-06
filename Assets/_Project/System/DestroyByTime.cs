using System;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    [SerializeField] private Timer timer;

    public void OnEnable()
    {
        timer.OnTimerEnd += OnTimerEnd;
    }

    public void OnDisable()
    {
        timer.OnTimerEnd -= OnTimerEnd;
    }

    private void OnTimerEnd()
    {
        Destroy(gameObject);
    }
}
