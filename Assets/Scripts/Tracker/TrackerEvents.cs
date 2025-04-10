using System;

public enum TrackerEventType { TRACKER_EVENT_NONE = -1, SESSION_START, SESSION_END, TRACKER_EVENT_LAST };

[Serializable]
public class TrackerEvent
{
    public string SessionId = "";
    public long Timestamp = 0;
    public int EventType = (int)TrackerEventType.TRACKER_EVENT_NONE;
    public string EventId = "";


    public TrackerEvent(string sessionId, int eventType)
    {
        SessionId = sessionId;
        Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        EventType = eventType;
        EventId = ((TrackerEventType)eventType).ToString() + "_" + Timestamp;
    }
}
