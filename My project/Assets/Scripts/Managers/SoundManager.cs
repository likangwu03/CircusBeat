using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ClipInfo
{
    public string name;
    public AudioClip audio;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    private double musicDuration;
    private double goalTime = 0.0;
    private double delay = 1.6;

    private AudioSource audioSource;
    private AudioClip currentClip = null;

    [SerializeField]
    private List<ClipInfo> clips;

    //public AudioClip currentClip;
    //public AudioSource audioSource;

    private void Awake()
    {
        // Patron Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        // Aregar un metodo a esta notificacion
        // (sus metodos se ejecutan cada vez que se carga una escena)
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip aux = null;
        foreach(ClipInfo clipInfo in clips)
        {
            if (clipInfo.name == scene.name)
            {
                aux = clipInfo.audio;
            }
        }

        if(currentClip != aux)
        {
            audioSource.Stop();
            goalTime = 0.0;
            delay = 1.6;
        }
        currentClip = aux;
    }

    // Update is called once per frame
    void Update()
    {
        if (AudioSettings.dspTime > goalTime - delay && currentClip != null)
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
