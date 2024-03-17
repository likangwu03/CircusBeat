using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private double musicDuration;
    private double goalTime;
    private double delay = 1.6;

    public AudioClip currentClip;
    public AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        if (AudioSettings.dspTime > goalTime - delay)
        {
            OnPlayMusic();
        }
    }

    private void OnPlayMusic()
    {
        goalTime = AudioSettings.dspTime - 1;

        audioSource.clip = currentClip;
        audioSource.PlayScheduled(goalTime);

        musicDuration = (double)currentClip.samples / currentClip.frequency;
        goalTime += musicDuration;
        delay += 0.2;
    }
}
