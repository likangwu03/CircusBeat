using System.Collections.Generic;

public interface IPersistence
{
    void SendEvent(TrackerEvent e);
    void SendEvents(IEnumerable<TrackerEvent> events);
    void Flush();
}

