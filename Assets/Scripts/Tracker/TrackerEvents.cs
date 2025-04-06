using System;

public enum TrackerEventType { TRACKER_EVENT_NONE = -1, SESSION_START, SESSION_END, TRACKER_EVENT_LAST };

[Serializable]
public class TrackerEvent
{
    public string SessionId { get; protected set; } = "";
    public long Timestamp { get; protected set; } = 0;
    public int EventType { get; protected set; } = (int)TrackerEventType.TRACKER_EVENT_NONE;
    public string EventId { get; protected set; } = "";


    public TrackerEvent(string sessionId, int eventType)
    {
        SessionId = sessionId;
        Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        EventType = eventType;
        EventId = ((TrackerEventType)eventType).ToString() + "_" + Timestamp;
    }
}
