using System;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public EffectProperties Properties { get; set; }

    protected float duration = 0f;
    public virtual void Start()
    {
        if (Properties == null)
        {
            enabled = false;
            return;
        }
        duration = Properties.duration;
    }

    public virtual void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            Destroy(this);
        }
    }
}
