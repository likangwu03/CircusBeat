using System;
using System.Collections.Generic;
using UnityEngine;


public class TrackerComponent : MonoBehaviour
{
    public static TrackerComponent Instance = null;

    public GameTracker Tracker { get; private set; }


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

        Tracker = new GameTracker(sessionId, maxQueueSize, persistenceMethods);
        Tracker.Open();


        //string evt1 = xml.Serialize(tracker.CreateGenericGameEvent(GameEventType.LEVEL_START));
        //string evt2 = xml.Serialize(tracker.CreateSongEndEvent(100, 12));
        //int a = 0;
    }

    private void OnApplicationQuit()
    {
        Tracker.Close();
    }

    public void SendEvent(TrackerEvent evt)
    {
        Tracker.SendEvent(evt);
    }
}
