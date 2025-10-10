using System;
using UnityEngine;

public class StunEffect : Effect
{
    private TargetFollower target;
    
    public override void Start()
    {
        base.Start();
        target = GetComponent<TargetFollower>();
        if (target != null)
            target.Stop();
    }

    public override void Update()
    {
        if (target != null)
            target.SpeedMultiplier = Properties.power;
        base.Update();
    }

    private void OnDestroy()
    {
        if (target != null) target.SpeedMultiplier = 1f;
    }
}
