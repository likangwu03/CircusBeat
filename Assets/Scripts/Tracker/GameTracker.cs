using System.Collections.Generic;

public class GameTracker : Tracker
{
    public GameTracker(string session, uint maxQueue, HashSet<BasePersistence> persistence, HashSet<int> eventsToIgnore) 
        : base(session, maxQueue, persistence, eventsToIgnore) { }


    public TrackerEvent CreateGenericGameEvent(GameEventType type)
    {
        return new GameEvent(sessionId, type, ref eventCounter);
    }

    public TrackerEvent CreateSongEndEvent(int score, int secondsPlayed)
    {
        return new SongEndEvent(sessionId, GameEventType.SONG_END, ref eventCounter, score, secondsPlayed);
    }
    public TrackerEvent CreatePlayerDeathEvent(int score, int secondsPlayed)
    {
        return new PlayerDeathEvent(sessionId, GameEventType.PLAYER_DEATH, ref eventCounter, score, secondsPlayed);
    }
    public TrackerEvent CreatePhaseChangeEvent(int phase)
    {
        return new PhaseChangeEvent(sessionId, GameEventType.PHASE_CHANGE, ref eventCounter, phase);
    }

    public enum ObstacleAction { SPAWN, DODGE, COLLISION };
    public TrackerEvent CreateObstacleEvent(string obs, int tr, ObstacleAction act)
    {
        GameEventType type = GameEventType.NULL;
        switch (act) {
            case ObstacleAction.SPAWN: type = GameEventType.OBSTACLE_SPAWN; break;
            case ObstacleAction.DODGE: type = GameEventType.OBSTACLE_DODGE; break;
            case ObstacleAction.COLLISION: type = GameEventType.OBSTACLE_COLLISION; break;
        }

        return new ObstacleEvent(sessionId, type, ref eventCounter, obs, tr);
    }

    public TrackerEvent CreateMovementEvent(MovementEvent.MovementType dir)
    {
        return new MovementEvent(sessionId, GameEventType.PLAYER_MOVEMENT, ref eventCounter, dir);
    }

}
