using System;
using System.Collections.Generic;
using UnityEngine;


public class TrackerComponent : MonoBehaviour
{
    public static TrackerComponent Instance = null;

    Tracker tracker;

    [SerializeField]
    uint maxQueueSize = 400;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            tracker = new Tracker(Environment.MachineName + "_" + DateTimeOffset.Now.ToUnixTimeSeconds(), maxQueueSize);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SendEvent(TrackerEvent evt)
    {
        tracker.SendEvent(evt);
    }
}
