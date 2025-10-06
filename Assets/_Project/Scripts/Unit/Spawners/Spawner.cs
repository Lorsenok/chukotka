using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected GameObject[] prefabs;
    [SerializeField] protected Timer timerDelay;
    [SerializeField] protected Timer rowDelay;
    [SerializeField] protected int rowTimes = 1;
    [SerializeField] protected int maxEntities = 10;

    protected int curRowTimes = 0;
    
    protected List<GameObject> spawned = new List<GameObject>();
    
    public virtual void Spawn()
    {
        List<int> ids = new List<int>();
        for (int i = 0; i < spawned.Count - 1; i++)
        {
            if (!spawned[i]) ids.Add(i);
        }
        foreach (int id in ids)
        {
            spawned.Remove(spawned[id]);
        }

        if (spawned.Count >= maxEntities) return;
        
        int rand = Random.Range(0, prefabs.Length);
        spawned.Add(Instantiate(prefabs[rand], transform.position, Quaternion.identity));
    }

    private void OnRowDelayEnd()
    {
        if (curRowTimes <= 0) return;
        curRowTimes--;
        Spawn();
    }

    private void OnDelayEnd()
    {
        curRowTimes = rowTimes;
    }

    public virtual void OnEnable()
    {
        timerDelay.OnTimerEnd += OnDelayEnd;
        rowDelay.OnTimerEnd += OnRowDelayEnd;
    }

    public virtual void OnDisable()
    {
        timerDelay.OnTimerEnd -= OnDelayEnd;
        rowDelay.OnTimerEnd -= OnRowDelayEnd;
    }
}
