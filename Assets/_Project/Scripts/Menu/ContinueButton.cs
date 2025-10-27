using UnityEngine;

public class ContinueButton : GameButton
{
    public override void Action()
    {
        base.Action();
        if (!isMouseOn) return;
        sceneChanger.LoadGame();
        enabled = false;
    }
}