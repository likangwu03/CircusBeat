using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public double musicDuration;
    public double goalTime;

    public AudioClip currentClip;
    public AudioSource audioSource;

    private double delay = 1.6;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(AudioSettings.dspTime);
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
