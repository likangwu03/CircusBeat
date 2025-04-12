
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
    protected uint selectPersistence;
    protected ulong eventCounter;

    public Tracker(string session, uint maxQueue, BasePersistence[] persistence)
    {
        sessionId = session;
        maxQueueSize = maxQueue;

        persistenceMethods = persistence;
        selectPersistence = 0;

        eventsQueue = new Queue<TrackerEvent>();

        eventCounter = 0;
    }

    public void SetPersistence(uint n)
    {
        selectPersistence = n;
    }

    public void Open()
    {
        persistenceMethods[selectPersistence].Start();
        SendEvent(CreateTrackerEvent(TrackerEventType.SESSION_START));
    }

    public void Close()
    {
        SendEvent(CreateTrackerEvent(TrackerEventType.SESSION_END), false);
        persistenceMethods[selectPersistence].Release();
    }

    public void SendEvent(TrackerEvent evt, bool delay = true)
    {
        eventsQueue.Enqueue(evt);
        if (eventsQueue.Count > maxQueueSize || !delay)
        {
            persistenceMethods[selectPersistence].SendEvents(eventsQueue);
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
