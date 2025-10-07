using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    private EffectProperties effectProps;
    public EffectProperties EffectProps
    {
        get
        {
            return effectProps;
        }
        set
        {
            effectTimer.StartTimer();
            giveEffect = true;
            effectProps = value;
        }
    }

    private Effect curEffect;
    public Effect CurEffect
    {
        get
        {
            return curEffect;
        }
        set
        {
            if (curEffect != null) Destroy(curEffect);
            curEffect = value;
        }
    }

    private void OnEffectEnd()
    {
        giveEffect = false;
    }

    [SerializeField] private GameObject[] ignoreObjects;
    [SerializeField] private int damage;
    [SerializeField] private GameObject[] spawnOnDamage;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool spawnOnDamageObject;
    [SerializeField] private float shakeOnDamage;

    [SerializeField] private Timer timerDelay;
    [SerializeField] private Timer effectTimer;
    private bool giveEffect = false;
    
    private List<DestroyableObject> curCollisions = new List<DestroyableObject>();
    
    private void OnEnable()
    {
        if (timerDelay != null) timerDelay.OnTimerEnd += DelayedDamage;
        if (effectTimer != null) effectTimer.OnTimerEnd += OnEffectEnd;
    }
    
    private void OnDisable()
    {
        if (timerDelay != null) timerDelay.OnTimerEnd -= DelayedDamage;
        if (effectTimer != null) effectTimer.OnTimerEnd -= OnEffectEnd;
    }

    private void DelayedDamage()
    {
        foreach (var obj in curCollisions)
        {
            DealDamage(obj);
        }
    }

    private void DealDamage(DestroyableObject obj)
    {
        obj.HP -= damage;
        foreach (GameObject spawn in spawnOnDamage)
        {
            Instantiate(spawn, spawnOnDamageObject ? obj.transform.position : spawnPoint.position, spawn.transform.rotation);
        }
        CameraMovement.Shake(shakeOnDamage);
        if (giveEffect) EffectGiver.AddComponent(obj.gameObject, curEffect, EffectProps);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.TryGetComponent(out DestroyableObject obj);
        if (obj == null || ignoreObjects.Contains(obj.gameObject)) return;
        curCollisions.Add(obj);
        DealDamage(obj);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        other.TryGetComponent(out DestroyableObject obj);
        if (obj == null || ignoreObjects.Contains(obj.gameObject)) return; 
        curCollisions.Remove(obj);
    }
}
