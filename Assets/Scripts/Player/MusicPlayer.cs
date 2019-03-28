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
    AudioSource audioSource;
    /// <summary>
    /// Amount of clips to load from the musicDir
    /// </summary>
    int clipAmount = 2;

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

    public string biom;

    World world;

    /// <summary>
    /// Loads all the music tracks into an array on start
    /// </summary>
    void Start()
    {
        world = GameObject.Find("World").GetComponent<World>();
        audioSource = this.GetComponent<AudioSource>();
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
        if (biom != world.GetChunk((int)transform.position.x, 0, (int)transform.position.z).biom)
        {
            biom = world.GetChunk((int)transform.position.x, 0, (int)transform.position.z).biom;

            if(biom == "wald")
            {
                currentClip = 1;
            }
            else if(biom == "grassland")
            {        
                currentClip = 0;
            }

            audioSource.Stop();
            nextPlay = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        if (clipAmount >= 0 && nextPlay <= DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
        {
            audioSource.clip = clips[currentClip];
            audioSource.Play();

            nextPlay = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + (int)(audioSource.clip.length * 1000) + delay;

        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            audioSource.Stop();
            nextPlay = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}