using System.Collections.Generic;
using System.IO;
using System.Threading;

public interface IPersistence
{
    void SendEvent(TrackerEvent e);
    void SendEvents(IEnumerable<TrackerEvent> events);
    void Flush();
    void Start();
    void Release();
}

