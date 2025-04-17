using UnityEngine;

public class DecoratorTest : MonoBehaviour
{
    private TrackerComponent trackerComp;
    private TrackerEventDecorator decorator;

    // Start is called before the first frame update
    void Start()
    {
        trackerComp = TrackerComponent.Instance;
        decorator = new DelayedEventDecorator(5000);
    }

    // Update is called once per frame
    void Update()
    {
        // La parte interesante de los Decorators es que puedes anidarlos para agregar
        // funcionalidad sobre un objeto en tiempo de ejecucion
        TrackerEvent trackerEvent = trackerComp.Tracker.CreateGenericGameEvent(GameEventType.LEVEL_START);
        decorator.ChainEvent(trackerEvent);
        trackerComp.SendEvent(decorator);
    }
}
