using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private string tagname;
    [SerializeField] private bool repeatable;

    public Action OnTimerEnd { get; set; }

    public void StartTimer()
    {
        curTime = timeSet;
    }

    [SerializeField] protected float timeSet;
    [SerializeField] private float curTime;

    private void Update()
    {
        if (curTime > 0f) curTime -= Time.deltaTime;
        else
        {
            OnTimerEnd?.Invoke();
            if (repeatable) StartTimer();
        }
    }
}
