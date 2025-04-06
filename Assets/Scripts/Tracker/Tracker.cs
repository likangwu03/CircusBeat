using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Tracker : MonoBehaviour
{
    public static Tracker Instance = null;


    string sessionId;
    [SerializeField]
    uint maxQueueSize = 400;
    Queue<TrackerEvent> eventsQueue;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sessionId = Environment.MachineName + "_" + DateTimeOffset.Now.ToUnixTimeSeconds();

        eventsQueue = new Queue<TrackerEvent>();
        TestCreateEvent();

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TestCreateEvent()
    {
        TrackerEvent ev = new TrackerEvent(sessionId, (int)TrackerEventType.SESSION_START);
        TrackerEvent lol = new GameEvent(sessionId, GameEventType.LEVEL_END);
        int asa = 1;
    }

    private void EnqueEvent(TrackerEvent evt)
    {
        eventsQueue.Enqueue(evt);
        if (eventsQueue.Count > maxQueueSize)
        {
            eventsQueue.Dequeue();
        }
    }
}
