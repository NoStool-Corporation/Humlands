using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
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
    int clipAmount = 3;

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

    public string biome;

    World world;

    /// <summary>
    /// Loads all the music tracks into an array on start
    /// </summary>
    void Start()
    {
		if (GameObject.Find("World") != null)
			world = GameObject.Find("World").GetComponent<World>();
        audioSource = this.GetComponent<AudioSource>();
        clips = new AudioClip[clipAmount];
        for (int i = 0; i < clipAmount; i++)
        {
            clips[i] = Resources.Load<AudioClip>(musicDir + i);
        }

        nextPlay = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
    /// <summary>
    /// Checks if there are music tracks and if it's time to play the next.
    /// </summary>
    void Update()
    {
		Chunk tmp = null;
		if (SceneManager.GetActiveScene().name=="Main")
			tmp = world.GetChunk((int)transform.position.x, 0, (int)transform.position.z);
		currentClip = 2;
		if (tmp != null && biome != tmp.biome && SceneManager.GetActiveScene().name=="Main")
        {
            biome = tmp.biome;

            if(biome == "forest")
            {
                currentClip = 0;
            }
            else if(biome == "grassland")
            {        
                currentClip = 1;
            }
			audioSource.Stop();
            nextPlay = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }    
		else if (!audioSource.isPlaying){
			currentClip = 2;
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