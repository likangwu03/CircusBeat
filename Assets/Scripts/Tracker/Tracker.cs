using System;
using System.Collections.Generic;

public enum TrackerEventType
{
    IGNORE_ALL = -2,
    NULL = -1,
    SESSION_START,
    SESSION_END,
    TRACKER_EVENT_LAST      // No se usa; es para saber a partir de que numero empiezan los eventos del juego
};


public interface ITrackerEvent
{
    void Send(Tracker tracker, bool delay = true);
}

[Serializable]
public class TrackerEvent : ITrackerEvent
{
    public string sessionId = "";
    public long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
    public int eventType = (int)TrackerEventType.NULL;
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

    public virtual void Send(Tracker tracker, bool delay = true)
    {
        tracker.SendEvent(this, delay);
    }
}

public class Tracker
{
    protected string sessionId;

    protected Queue<TrackerEvent> eventsQueue;
    protected uint maxQueueSize;
    protected ulong eventCounter;

    protected HashSet<BasePersistence> persistenceMethods = new HashSet<BasePersistence>();
    protected HashSet<int> ignoredEvents = new HashSet<int>();

    public Tracker(string session, uint maxQueue, HashSet<BasePersistence> persistence, HashSet<int> eventsToIgnore)
    {
        sessionId = session;

        eventsQueue = new Queue<TrackerEvent>();
        maxQueueSize = maxQueue;
        eventCounter = 0;

        persistenceMethods = persistence;
        ignoredEvents = eventsToIgnore;
    }


    public void Open()
    {
        // Si se van a ignorar todos los eventos, no se hace nada
        if (ignoredEvents.Contains((int)TrackerEventType.IGNORE_ALL))
        {
            return;
        }

        foreach (BasePersistence method in persistenceMethods)
        {
            method.Start();
        }
        SendEvent(CreateGenericTrackerEvent(TrackerEventType.SESSION_START));
    }

    public void Close()
    {
        // Si se van a ignorar todos los eventos, no se hace nada
        if (ignoredEvents.Contains((int)TrackerEventType.IGNORE_ALL))
        {
            return;
        }

        SendEvent(CreateGenericTrackerEvent(TrackerEventType.SESSION_END), false);
        foreach (BasePersistence method in persistenceMethods)
        {
            method.Release();
        }
    }

    public void SendEvent(TrackerEvent evt, bool delay = true)
    {
        // Si se van a ignorar todos los eventos, no se hace nada
        if (ignoredEvents.Contains((int)TrackerEventType.IGNORE_ALL))
        {
            return;
        }
        // Si no se va a ignorar el tipo del evento, se mete a la cola
        else if (!ignoredEvents.Contains(evt.eventType))
        {
            eventsQueue.Enqueue(evt);
        }

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
