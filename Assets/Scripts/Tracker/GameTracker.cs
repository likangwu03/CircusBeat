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
}
