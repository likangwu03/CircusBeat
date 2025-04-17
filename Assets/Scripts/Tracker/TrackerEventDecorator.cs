public class TrackerEventDecorator : ITrackerEvent
{
    private ITrackerEvent trackerEvent;

    public void ChainEvent(ITrackerEvent trackerEvent)
    {
        this.trackerEvent = trackerEvent;
    }

    public virtual void Send(Tracker tracker, bool delay = true)
    {
        if (trackerEvent != null)
        {
            trackerEvent.Send(tracker, delay);
        }
    }
}
