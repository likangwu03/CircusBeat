using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistence
{
    void SendEvent(TrackerEvent e);
    void SendEvents(IEnumerable<TrackerEvent> events);
    void Flush();
}

