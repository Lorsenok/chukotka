using UnityEngine;

public class NewGameButton : GameButton
{
    public override void Action()
    {
        if (!isMouseOn) return;
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("GameOn", 1);
        PlayerPrefs.Save();
        base.Action();
    }
}
