using UnityEngine;

public class NewGameButton : GameButton
{
    public override void Action()
    {
        if (!isMouseOn) return;
        PlayerPrefs.DeleteAll();
        base.Action();
    }
}
