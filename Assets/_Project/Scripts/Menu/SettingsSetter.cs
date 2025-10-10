using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSetter : MonoBehaviour
{
    public static Action OnSettingsApply { get; set; }

    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider shakeSlider;
    [SerializeField] private Slider postProcessingSlider;

    public void Apply()
    {
        Config.Sound = soundSlider.value;
        Config.Music = musicSlider.value;
        Config.ShakePower = shakeSlider.value;
        Config.PostProcessing = postProcessingSlider.value;

        OnSettingsApply?.Invoke();

        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("sound", Config.Sound);
        PlayerPrefs.SetFloat("music", Config.Music);
        PlayerPrefs.SetFloat("shake", Config.ShakePower);
        PlayerPrefs.SetFloat("postprocessing", Config.PostProcessing);
    }

    private void Load()
    {
        if (!PlayerPrefs.HasKey("sound"))
        {
            soundSlider.value = Config.DefaultSound;
            musicSlider.value = Config.DefaultMusic;
            shakeSlider.value = Config.DefaultShakePower;
            postProcessingSlider.value = Config.DefaultPostProcessing;
            Apply();
            return;
        }

        float sound = PlayerPrefs.GetFloat("sound");
        float music = PlayerPrefs.GetFloat("music");
        float shake = PlayerPrefs.GetFloat("shake");
        float postProcessing = PlayerPrefs.GetFloat("postprocessing");

        soundSlider.value = sound;
        musicSlider.value = music;
        shakeSlider.value = shake;
        postProcessingSlider.value = postProcessing;
    }

    private void Awake()
    {
        Load();
        Apply();
    }
}
