using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip cutsceneMusic;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        AudioClip targetClip;

        if (sceneName == "Menu")
        {
            targetClip = menuMusic;
        }
        else if (sceneName == "Cutscene1")
        {
            targetClip = cutsceneMusic;
        }
        else
        {
            targetClip = gameMusic;
        }

        if (audioSource.clip != targetClip)
        {
            StartCoroutine(FadeToNewClip(targetClip));
        }
        else
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }


    private IEnumerator FadeToNewClip(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0f;

        audioSource.clip = newClip;
        audioSource.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, startVolume, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = startVolume;
    }
}
