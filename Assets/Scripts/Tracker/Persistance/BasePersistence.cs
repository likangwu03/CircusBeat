using System.Collections.Concurrent;
using System.Collections.Generic;

public class BasePersistence : IPersistence
{
    protected ConcurrentQueue<TrackerEvent> eventQueue;
    protected ISerializer serializer;

    public BasePersistence(string sessionId, ISerializer serializer)
    {
        eventQueue = new ConcurrentQueue<TrackerEvent>();
        this.serializer = serializer;
    }

    public virtual void Start() { }
    public virtual void Release() { }
    public virtual void SendEvent(TrackerEvent e) { }
    public virtual void SendEvents(IEnumerable<TrackerEvent> events) { }
    public virtual void Flush() { }

}
