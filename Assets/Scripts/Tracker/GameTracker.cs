using static ObstacleEvent;

public class GameTracker : Tracker
{
    public GameTracker(string session, uint maxQueue, BasePersistence[] persistence) : base(session, maxQueue, persistence) { }


    public TrackerEvent CreateGenericGameEvent(GameEventType type)
    {
        return new GameEvent(sessionId, type, ref eventCounter);
    }

    public TrackerEvent CreateSongEndEvent(int score, int secondsPlayed)
    {
        return new SongEndEvent(sessionId, GameEventType.SONG_END, ref eventCounter, score, secondsPlayed);
    }
    public TrackerEvent CreatePlayerDeathEvent(int score,int secondsPlayed)
    {
        return new PlayerDeathEvent(sessionId, GameEventType.PLAYER_DEATH, ref eventCounter, score, secondsPlayed);
    }
    public TrackerEvent CreatePhaseChangeEvent(int phase)
    {
        return new PhaseChangeEvent(sessionId, GameEventType.PHASE_CHANGE, ref eventCounter, phase);
    }

    public TrackerEvent CreateObstacleEvent(string obs, int tr,ObstacleAction act)
    {
        return new ObstacleEvent(sessionId, GameEventType.OBSTACLE_SPAWN, ref eventCounter, obs, tr,act);
    }

    public TrackerEvent CreateMovementEvent(MovementEvent.MovementType dir)
    {
        return new MovementEvent(sessionId, GameEventType.MOVEMENT, ref eventCounter, dir);
    }
  

}
