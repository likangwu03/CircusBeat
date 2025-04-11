
using System;
using System.Collections.Generic;

public enum TrackerEventType { TRACKER_EVENT_NONE = -1, SESSION_START, SESSION_END, TRACKER_EVENT_LAST };
[Serializable]
public class TrackerEvent
{
    public string SessionId = "";
    public long Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
    public string EventName = "";
    public ulong EventId = 0;
}

public class Tracker
{
    protected string sessionId;
    protected uint maxQueueSize = 400;
    protected Queue<TrackerEvent> eventsQueue;
    protected private ThreadedEventLogger threadedEvent;
    protected BasePersistence[] persistenceMethods;

    protected ulong eventCounter;

    public Tracker(string session, uint maxQueue, BasePersistence[] persistence)
    {
        sessionId = session;
        maxQueueSize = maxQueue;

        persistenceMethods = persistence;

        eventsQueue = new Queue<TrackerEvent>();
        //threadedEvent = new ThreadedEventLogger(sessionId);
        //threadedEvent.Start();

        eventCounter = 0;
    }

    public void Close()
    {
        SendEvent(CreateTrackerEvent(TrackerEventType.SESSION_END));
        //threadedEvent.WriteEvent(eventsQueue);
        //threadedEvent.Destroy();
        eventsQueue.Clear();
    }

    public void SendEvent(TrackerEvent evt)
    {
        eventsQueue.Enqueue(evt);
        //if (eventsQueue.Count > maxQueueSize)
        //{
        //    threadedEvent.WriteEvent(eventsQueue);
        //    eventsQueue.Clear();
        //}
    }


    private TrackerEvent CreateTrackerEvent(TrackerEventType type)
    {
        TrackerEvent evt = new TrackerEvent { SessionId = sessionId, EventName = type.ToString(), EventId = eventCounter };
        eventCounter++;
        return evt;
    }
}
