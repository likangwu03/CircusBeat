using System;
using System.Collections.Generic;
using UnityEngine;


public class TrackerComponent : MonoBehaviour
{
    public static TrackerComponent Instance = null;

    GameTracker tracker;

    [SerializeField]
    uint maxQueueSize = 400;


    enum SerializeMethods { JSON, XML, BINARY };
    [SerializeField]
    SerializeMethods serializeMethod;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            CreateAndConfigureTracker();
        }
        else
        {
            Destroy(this);
        }
    }


    public void CreateAndConfigureTracker()
    {
        string sessionId = Environment.MachineName + "_" + DateTimeOffset.Now.ToUnixTimeSeconds();
        BasePersistence[] persistenceMethods =
        {
            new LocalPersistence(sessionId, new JsonSerializer()),
            new LocalPersistence(sessionId, new XMLSerializer())
        };

        tracker = new GameTracker(sessionId, maxQueueSize, persistenceMethods);


        XMLSerializer asda = new XMLSerializer();
        JsonSerializer oooo = new JsonSerializer();
        string huh = asda.Serialize(tracker.CreateGameEvent(GameEventType.LEVEL_START));
        string bro = oooo.Serialize(tracker.CreateGameEvent(GameEventType.LEVEL_END));
        int das = 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnApplicationQuit()
    {
        tracker.Close();
    }
    void SendEvent(TrackerEvent evt)
    {
        tracker.SendEvent(evt);
    }
}
