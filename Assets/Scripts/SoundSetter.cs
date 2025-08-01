using UnityEngine;

public class SoundSetter : MonoBehaviour
{
    [SerializeField] private AudioSource[] sounds;
    [SerializeField] private AudioSource[] music;

    private void Awake()
    {
        UpdateVolume();
        SettingsSetter.OnSettingsApply += UpdateVolume;
    }

    private void OnDestroy()
    {
        SettingsSetter.OnSettingsApply -= UpdateVolume;
    }

    private void UpdateVolume()
    {
        foreach (var sound in sounds)
        {
            sound.volume = Config.Sound;
        }

        foreach (var music in music)
        {
            music.volume = Config.Music;
        }
    }
}
