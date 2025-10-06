using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float SpeedMultiplier { get; set; } = 1f;
    
    [SerializeField] private string tagname;
    [SerializeField] protected bool repeatable;

    public Action OnTimerEnd { get; set; }

    public virtual void StartTimer()
    {
        curTime = timeSet;
    }

    [SerializeField] protected string savekey;

    [SerializeField] protected float timeSet;
    [SerializeField] protected float curTime;

    public void Start()
    {
        if (PlayerPrefs.HasKey(savekey)) curTime = PlayerPrefs.GetFloat(savekey);
    }

    public virtual void Update()
    {
        if (savekey != "" && savekey != string.Empty) PlayerPrefs.SetFloat(savekey, curTime);
        if (curTime > 0f) curTime -= Time.deltaTime * SpeedMultiplier;
        else
        {
            OnTimerEnd?.Invoke();
            if (repeatable) StartTimer();
        }
    }
}
