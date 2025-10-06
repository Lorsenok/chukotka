using System;
using UnityEngine;

public class BleedEffect : Effect
{
    private DestroyableObject target;
    private float timeFromStart = 0f;
    
    public override void Start()
    {
        base.Start();
        target = GetComponent<DestroyableObject>();
    }

    private int lastDamageDealtTime = 0;
    public override void Update()
    {
        if (target == null)
        {
            Destroy(this);
            return;
        }
        base.Update();
        timeFromStart += Time.deltaTime;
        if ((int)timeFromStart > lastDamageDealtTime)
        {
            lastDamageDealtTime = (int)timeFromStart;
            target.HP -= (int)Properties.power;
        }
    }
}
