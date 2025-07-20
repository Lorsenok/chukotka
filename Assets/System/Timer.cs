using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private string tagname;
    [SerializeField] protected bool repeatable;

    public Action OnTimerEnd { get; set; }

    public virtual void StartTimer()
    {
        curTime = timeSet;
    }

    [SerializeField] protected float timeSet;
    [SerializeField] protected float curTime;

    public virtual void Update()
    {
        if (curTime > 0f) curTime -= Time.deltaTime;
        else
        {
            OnTimerEnd?.Invoke();
            if (repeatable) StartTimer();
        }
    }
}
