using UnityEngine;

public class TriggerByTimer : Trigger
{
    [SerializeField] private Timer timer;

    public override void OnEnable()
    {
        timer.OnTimerEnd += Action;
        base.OnEnable();
    }

    public override void OnDisable()
    {
        timer.OnTimerEnd -= Action;
        base.OnDisable();
    }
}
