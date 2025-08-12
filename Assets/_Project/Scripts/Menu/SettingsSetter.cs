using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSetter : MonoBehaviour
{
    public static Action OnSettingsApply { get; set; }

    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;

    public void Apply()
    {
        Config.Sound = soundSlider.value;
        Config.Music = musicSlider.value;

        OnSettingsApply?.Invoke();

        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("sound", Config.Sound);
        PlayerPrefs.SetFloat("music", Config.Music);
    }

    private void Load()
    {
        if (!PlayerPrefs.HasKey("sound"))
        {
            soundSlider.value = Config.DefaultSound;
            musicSlider.value = Config.DefaultMusic;
            Apply();
            return;
        }

        float sound = PlayerPrefs.GetFloat("sound");
        float music = PlayerPrefs.GetFloat("music");

        soundSlider.value = sound;
        musicSlider.value = music;
    }

    private void Awake()
    {
        Load();
        Apply();
    }
}
