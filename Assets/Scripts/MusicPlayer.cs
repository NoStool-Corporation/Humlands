using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    int clipAmount = 1;
    AudioClip[] clips;
    int currentClip;
    int delay = 30000;
    long nextPlay;

    String musicDir = "Music/";

    void Start()
    {
        audioSource.volume = 0.01f;
        clips = new AudioClip[clipAmount];
        for (int i = 0; i < clipAmount; i++)
        {
            print(musicDir + i + ".mp3");
            clips[i] = Resources.Load<AudioClip>(musicDir + i);
        }

        nextPlay = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        currentClip = 0;
    }

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