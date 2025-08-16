using System;
using UnityEngine;

public class DialogueButton : GameButton
{
    public int Id { get; set; }
    public static Action<int> OnButtonPressed { get; set; }

    [SerializeField] private Timer blockTimer;
    private bool block = true;

    public override void OnEnable()
    {
        base.OnEnable();
        blockTimer.OnTimerEnd += OnBlockEnd;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        blockTimer.OnTimerEnd -= OnBlockEnd;
    }

    private void OnBlockEnd()
    {
        block = false;
    }

    public override void Action()
    {
        if (block) return;

        base.Action();
        if (isMouseOn)
        {
            OnButtonPressed?.Invoke(Id);
        }
    }
}
