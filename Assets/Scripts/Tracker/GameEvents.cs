using System;

public enum GameEventType { LEVEL_START = TrackerEventType.TRACKER_EVENT_LAST, LEVEL_END };

[Serializable]
public class GameEvent : TrackerEvent
{
    public GameEvent(string sessionId, GameEventType eventType) : base(sessionId, (int)eventType)
    {
        EventId = eventType.ToString() + "_" + Timestamp;
    }
}
