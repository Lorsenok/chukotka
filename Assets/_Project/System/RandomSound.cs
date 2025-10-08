using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class RandomSound : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioResource[] resources;
    [SerializeField] private bool playOnStart = true;
    
    public void SetSound()
    {
        audio.resource = resources[Random.Range(0, resources.Length - 1)];
    }

    private void Start()
    {
        SetSound();
        if (playOnStart) audio.Play();
    }
}
