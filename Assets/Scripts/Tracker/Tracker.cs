using System;
using System.Collections.Generic;

public enum TrackerEventType
{
    NONE = -1,
    SESSION_START,
    SESSION_END,
    TRACKER_EVENT_LAST      // No se usa; es para saber a partir de que numero empiezan los eventos del juego
};


[Serializable]
public class TrackerEvent
{
    public string sessionId = "";
    public long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
    public int eventType = (int)TrackerEventType.NONE;
    public string eventName = "";
    public ulong eventId = 0;

    // IMPORTANTE: PARA QUE LA SERIALIZACION EN XML FUNCIONE, ES NECESARIO QUE
    // TODOS LOS EVENTOS Y SUS CLASES HIJAS TENGAN CONSTRUCTORA SIN PARAMETROS
    public TrackerEvent() { }
    public TrackerEvent(string session, TrackerEventType type, ref ulong counter)
    {
        sessionId = session;
        timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        eventType = (int)type;
        eventName = type.ToString();
        eventId = counter;

        counter++;
    }
}


public class Tracker
{
    protected string sessionId;

    protected Queue<TrackerEvent> eventsQueue;
    protected uint maxQueueSize;
    protected ulong eventCounter;

    protected BasePersistence[] persistenceMethods;

    public Tracker(string session, uint maxQueue, BasePersistence[] persistence)
    {
        sessionId = session;

        eventsQueue = new Queue<TrackerEvent>();
        maxQueueSize = maxQueue;
        eventCounter = 0;

        persistenceMethods = persistence;
    }


    public void Open()
    {
        foreach (BasePersistence method in persistenceMethods)
        {
            method.Start();
        }
        SendEvent(CreateGenericTrackerEvent(TrackerEventType.SESSION_START));
    }

    public void Close()
    {
        SendEvent(CreateGenericTrackerEvent(TrackerEventType.SESSION_END), false);
        foreach (BasePersistence method in persistenceMethods)
        {
            method.Release();
        }
    }

    public void SendEvent(TrackerEvent evt, bool delay = true)
    {
        eventsQueue.Enqueue(evt);
        if (eventsQueue.Count > maxQueueSize || !delay)
        {
            foreach (BasePersistence method in persistenceMethods)
            {
                method.SendEvents(eventsQueue);
            }
            eventsQueue.Clear();
        }
    }

    protected TrackerEvent CreateGenericTrackerEvent(TrackerEventType type)
    {
        return new TrackerEvent(sessionId, type, ref eventCounter);
    }
}
