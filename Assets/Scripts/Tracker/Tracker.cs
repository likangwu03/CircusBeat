
using System.Collections.Generic;

public class Tracker
{
    string sessionId;
    uint maxQueueSize = 400;
    Queue<TrackerEvent> eventsQueue;
    private ThreadedEventLogger threadedEvent;

    public Tracker(string session, uint maxQueue)
    {
        sessionId = session;
        maxQueueSize = maxQueue;

        eventsQueue = new Queue<TrackerEvent>();
        threadedEvent = new ThreadedEventLogger(sessionId);
        threadedEvent.Start();
    }

    ~Tracker()
    {
        UnityEngine.Debug.Log("socorro");
        TrackerEvent ev = new TrackerEvent(sessionId, (int)TrackerEventType.SESSION_END);
        SendEvent(ev);
        threadedEvent.WriteEvent(eventsQueue);
        threadedEvent.Destroy();
        eventsQueue.Clear();
    }

    public void SendEvent(TrackerEvent evt)
    {
        eventsQueue.Enqueue(evt);
        if (eventsQueue.Count > maxQueueSize)
        {
            threadedEvent.WriteEvent(eventsQueue);
            eventsQueue.Clear();
        }
    }

}
