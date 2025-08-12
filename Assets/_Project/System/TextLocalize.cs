using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class TextLocalize : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private string key;
    [SerializeField] private LocalizedStringTable table;

    private void Translate(Locale locale)
    {
        text.text = table.GetTable().GetEntry(key).Value;
    }

    private void Start()
    {
        text.text = table.GetTable().GetEntry(key).Value;
    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += Translate;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= Translate;
    }
}
