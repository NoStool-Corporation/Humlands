using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Plays the different music tracks based on the state of the game
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    /// <summary>
    /// Amount of clips to load from the musicDir
    /// </summary>
    int clipAmount = 1;

    AudioClip[] clips;

    /// <summary>
    /// The index of the current clip
    /// </summary>
    int currentClip;

    /// <summary>
    /// The delay in ms between the end and the start of two music clips
    /// </summary>
    int delay = 30000;

    long nextPlay;

    String musicDir = "Music/";

    /// <summary>
    /// Loads all the music tracks into an array on start
    /// </summary>
    void Start()
    {
        audioSource.volume = 0.01f;
        clips = new AudioClip[clipAmount];
        for (int i = 0; i < clipAmount; i++)
        {
            clips[i] = Resources.Load<AudioClip>(musicDir + i);
        }

        nextPlay = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        currentClip = 0;
    }
    /// <summary>
    /// Checks if there are music tracks and if it's time to play the next.
    /// </summary>
    void Update()
    {
        if (clipAmount >= 0 && nextPlay <= DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
        {
            audioSource.clip = clips[currentClip];
            audioSource.Play();

            nextPlay = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + (int)(audioSource.clip.length * 1000) + delay;

            currentClip++;
            if(currentClip <= clipAmount)
            {
                currentClip = 0;
            }
        }
    }
}