
using System;

public enum GameEventType { LEVEL_START = TrackerEventType.TRACKER_EVENT_LAST, LEVEL_END };

public class GameTracker : Tracker
{

    [Serializable]
    private class GameEvent : TrackerEvent { }

    [Serializable]
    private class SongEndEvent : GameEvent { }

    

    public GameTracker(string session, uint maxQueue, BasePersistence[] persistence) : base(session,maxQueue,persistence) { }

    public TrackerEvent CreateGameEvent(GameEventType type)
    {
        GameEvent evt = new GameEvent { SessionId = sessionId, EventName = type.ToString(), EventId = eventCounter };
        eventCounter++;
        return evt;
    }
}
