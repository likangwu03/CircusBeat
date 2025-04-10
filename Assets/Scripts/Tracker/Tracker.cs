using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


public class Tracker : MonoBehaviour
{
    public static Tracker Instance = null;


    string sessionId;
    [SerializeField]
    uint maxQueueSize = 400;
    Queue<TrackerEvent> eventsQueue;

    private ThreadedEventLogger threadedEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sessionId = Environment.MachineName + "_" + DateTimeOffset.Now.ToUnixTimeSeconds();
        eventsQueue = new Queue<TrackerEvent>();
        threadedEvent = new ThreadedEventLogger();
        threadedEvent.Start();

        //TestCreateEvent();
        TrackerEvent ev = new TrackerEvent(sessionId, (int)TrackerEventType.SESSION_START);
        //SendEvent(ev);
        string tst = JsonUtility.ToJson(ev);
        int aaa = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SendEvent(TrackerEvent evt)
    {
        eventsQueue.Enqueue(evt);
        if (eventsQueue.Count > maxQueueSize)
        {
            DequeEvent();
        }
    }


    private void DequeEvent()
    {
        while (eventsQueue.Count > 0)
        {
            TrackerEvent evt = eventsQueue.Dequeue();
            threadedEvent.AddEvent(evt.ToString());
        }
    }

    private void OnApplicationQuit()
    {
        TrackerEvent ev = new TrackerEvent(sessionId, (int)TrackerEventType.SESSION_END);
        SendEvent(ev);
        DequeEvent();
        threadedEvent.OnDestroy();
    }

}
