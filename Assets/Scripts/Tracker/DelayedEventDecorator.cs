using System.Threading.Tasks;

public class DelayedEventDecorator : TrackerEventDecorator
{
    private int delayTime;
    private bool waiting;

    public DelayedEventDecorator(int delayTime = 1000)
    {
        this.delayTime = delayTime;
        waiting = false;
    }

    public override void Send(Tracker tracker, bool delay = true)
    {
        if (!waiting)
        {
            base.Send(tracker, delay);
            _ = Wait();
        }
    }

    // Hay que usar metodos asincronos porque las corrutinas son de MonoBehavior
    private async Task Wait()
    {
        waiting = true;
        await Task.Delay(delayTime);
        waiting = false;
    }
}
