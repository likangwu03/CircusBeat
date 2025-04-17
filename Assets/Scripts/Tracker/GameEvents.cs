using System;

public enum GameEventType
{
    LEVEL_START = TrackerEventType.TRACKER_EVENT_LAST,
    SONG_START,
    SONG_END,
    PLAYER_DEATH,
    LEVEL_END,
    LEVEL_QUIT,
    PHASE_CHANGE,
    OBSTACLE_ACTION,
    RECOVER_HEALTH,
    PLAYER_MOVEMENT
};

[Serializable]
public class GameEvent : TrackerEvent
{
    public GameEvent() : base() { }
    public GameEvent(string session, GameEventType type, ref ulong counter) : base(session, TrackerEventType.NONE, ref counter)
    {
        eventType = (int)type;
        eventName = type.ToString();
    }
}

[Serializable]
public class SongEndEvent : GameEvent
{
    public int score = 0;
    public int secondsPlayed = 0;

    public SongEndEvent() : base() { }

    public SongEndEvent(string session, GameEventType type, ref ulong counter, int finalScore, int playTime) : base(session, type, ref counter)
    {
        score = finalScore;
        secondsPlayed = playTime;
    }
}

[Serializable]
public class PlayerDeathEvent : GameEvent
{
    public int score = 0;
    public int secondsPlayed = 0;

    public PlayerDeathEvent() : base() { }
    public PlayerDeathEvent(string session, GameEventType type, ref ulong counter, int finalScore, int playTime) : base(session, type, ref counter)
    {
        score = finalScore;
        secondsPlayed = playTime;
    }
}


[Serializable]
public class PhaseChangeEvent : GameEvent
{
    public int newPhase = 0;

    public PhaseChangeEvent() : base() { }
    public PhaseChangeEvent(string session, GameEventType type, ref ulong counter, int phase) : base(session, type, ref counter)
    {
        newPhase = phase;
    }
}

[Serializable]
public class ObstacleEvent : GameEvent
{
    public enum ObstacleAction { NONE, SPAWN, EVASION, COLLISION };

    public string obstacle = " ";
    public string action = " ";
    public int track = 0;

    public ObstacleEvent() : base() { }
    public ObstacleEvent(string session, GameEventType type, ref ulong counter, string obs, int tr, ObstacleAction act) : base(session, type, ref counter)
    {
        obstacle = obs;
        track = tr;
        action = act.ToString();
    }
}

[Serializable]
public class MovementEvent : GameEvent
{
    public enum MovementType { NONE, LEFT, RIGHT, UP };
    public string direction = "";

    public MovementEvent() : base() { }
    public MovementEvent(string session, GameEventType type, ref ulong counter, MovementType dir) : base(session, type, ref counter)
    {
        direction = dir.ToString();
    }
}
