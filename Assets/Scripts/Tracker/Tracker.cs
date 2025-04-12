
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
    protected uint maxQueueSize;
    protected Queue<TrackerEvent> eventsQueue;
    protected BasePersistence[] persistenceMethods;

    protected ulong eventCounter;

    public Tracker(string session, uint maxQueue, BasePersistence[] persistence)
    {
        sessionId = session;
        maxQueueSize = maxQueue;

        persistenceMethods = persistence;

        eventsQueue = new Queue<TrackerEvent>();
     

        eventCounter = 0;
    }

    public void Close()
    {
        SendEvent(CreateTrackerEvent(TrackerEventType.SESSION_END), false);
    }

    public void SendEvent(TrackerEvent evt, bool delay = true)
    {
        eventsQueue.Enqueue(evt);
        if (eventsQueue.Count > maxQueueSize || !delay)
        {
            //TODO aqui hay un 0 
            persistenceMethods[0].SendEvents(eventsQueue);
            eventsQueue.Clear();
        }
    }

    private TrackerEvent CreateTrackerEvent(TrackerEventType type)
    {
        TrackerEvent evt = new TrackerEvent { SessionId = sessionId, EventName = type.ToString(), EventId = eventCounter };
        eventCounter++;
        return evt;
    }
}
