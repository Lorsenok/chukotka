using System;
using UnityEngine;

public class DialogueButton : GameButton
{
    public int Id { get; set; }
    public static Action<int> OnButtonPressed { get; set; }

    public override void Action()
    {
        base.Action();
        if (isMouseOn)
        {
            OnButtonPressed?.Invoke(Id);
        }
    }
}
