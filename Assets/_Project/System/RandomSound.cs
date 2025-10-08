using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class RandomSound : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioResource[] resources;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private Timer delay;

    private void OnDelayEnd()
    {
        Play();
    }

    private void OnEnable()
    {
        if (delay != null) delay.OnTimerEnd += OnDelayEnd;
    }

    private void OnDisable()
    {
        if (delay != null) delay.OnTimerEnd -= OnDelayEnd;
    }

    public void Play()
    {
        audio.resource = resources[Random.Range(0, resources.Length - 1)];
        audio.Play();
    }

    private void Start()
    {
        if (playOnStart) Play();
    }
}
