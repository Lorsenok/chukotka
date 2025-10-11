using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocaleButton : GameButton
{
    [SerializeField] private Locale locale;
    
    public override void Action()
    {
        base.Action();
        if (isMouseOn) LocalizationSettings.SelectedLocale = locale;
    }
}
