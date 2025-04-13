using System;
using System.Collections.Generic;
using UnityEngine;


public class TrackerComponent : MonoBehaviour
{
    public static TrackerComponent Instance = null;

    GameTracker tracker;


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

        JsonSerializer json = new JsonSerializer();
        XMLSerializer xml = new XMLSerializer();

        BasePersistence[] persistenceMethods =
        {
            new LocalPersistence(sessionId, json),
            //new LocalPersistence(sessionId, xml)

            // Para anadir otro metodo de serializacion habria que hacer hacer lo mismo:
            // crear una instancia del serializador del formato deseado y crear un nuevo
            // persistidor pasandole dicha instancia

            // Para anadir otro metodo de persistencia
            //new RemotePersistence(sessionId, json)    --> Si se implementara la persistencia en remoto
        };

        uint maxQueueSize = 400;

        tracker = new GameTracker(sessionId, maxQueueSize, persistenceMethods);
        tracker.Open();


        //string evt1 = xml.Serialize(tracker.CreateGenericGameEvent(GameEventType.LEVEL_START));
        //string evt2 = xml.Serialize(tracker.CreateSongEndEvent(100, 12));
        //int a = 0;
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

    public void SendEvent(TrackerEvent evt)
    {
        tracker.SendEvent(evt);
    }
}
